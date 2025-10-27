using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using PKHeX.Core;

namespace PLZASaveDumper;

public partial class MainForm : Form
{
    private TextBox txtSavePath = null!;
    private TextBox txtOutputDir = null!;
    private Button btnBrowseSave = null!;
    private Button btnBrowseOutput = null!;
    private Button btnDump = null!;
    private Button btnClear = null!;
    private RichTextBox txtLog = null!;
    private ProgressBar progressBar = null!;

    public MainForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.Text = "Pokemon Legends: Z-A Save File Dumper";
        this.Width = 900;
        this.Height = 700;
        this.StartPosition = FormStartPosition.CenterScreen;

        // Title Label
        var lblTitle = new Label
        {
            Text = "Pokemon Legends: Z-A Save File Dumper",
            Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
            AutoSize = true,
            Location = new System.Drawing.Point(20, 20)
        };
        this.Controls.Add(lblTitle);

        // Save File Selection
        var lblSave = new Label
        {
            Text = "Save File:",
            Location = new System.Drawing.Point(20, 70),
            AutoSize = true
        };
        this.Controls.Add(lblSave);

        txtSavePath = new TextBox
        {
            Location = new System.Drawing.Point(100, 67),
            Width = 650,
            ReadOnly = true
        };
        this.Controls.Add(txtSavePath);

        btnBrowseSave = new Button
        {
            Text = "Browse...",
            Location = new System.Drawing.Point(760, 65),
            Width = 100
        };
        btnBrowseSave.Click += BtnBrowseSave_Click;
        this.Controls.Add(btnBrowseSave);

        // Output Directory Selection
        var lblOutput = new Label
        {
            Text = "Output Dir:",
            Location = new System.Drawing.Point(20, 110),
            AutoSize = true
        };
        this.Controls.Add(lblOutput);

        txtOutputDir = new TextBox
        {
            Location = new System.Drawing.Point(100, 107),
            Width = 650,
            Text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dumps")
        };
        this.Controls.Add(txtOutputDir);

        btnBrowseOutput = new Button
        {
            Text = "Browse...",
            Location = new System.Drawing.Point(760, 105),
            Width = 100
        };
        btnBrowseOutput.Click += BtnBrowseOutput_Click;
        this.Controls.Add(btnBrowseOutput);

        // Action Buttons
        btnDump = new Button
        {
            Text = "Decrypt && Dump Save",
            Location = new System.Drawing.Point(300, 150),
            Width = 180,
            Height = 35
        };
        btnDump.Click += BtnDump_Click;
        this.Controls.Add(btnDump);

        btnClear = new Button
        {
            Text = "Clear Log",
            Location = new System.Drawing.Point(500, 150),
            Width = 100,
            Height = 35
        };
        btnClear.Click += (s, e) => txtLog.Clear();
        this.Controls.Add(btnClear);

        // Progress Bar
        progressBar = new ProgressBar
        {
            Location = new System.Drawing.Point(20, 200),
            Width = 840,
            Height = 25,
            Style = ProgressBarStyle.Continuous
        };
        this.Controls.Add(progressBar);

        // Log Area
        var lblLog = new Label
        {
            Text = "Log:",
            Location = new System.Drawing.Point(20, 240),
            AutoSize = true
        };
        this.Controls.Add(lblLog);

        txtLog = new RichTextBox
        {
            Location = new System.Drawing.Point(20, 265),
            Width = 840,
            Height = 360,
            ReadOnly = true,
            Font = new System.Drawing.Font("Consolas", 9)
        };
        this.Controls.Add(txtLog);

        Log("Ready. Select a save file to begin.");
    }

    private void BtnBrowseSave_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Select Save File",
            Filter = "All Files|*.*|Save Files (*.bin)|*.bin",
            FilterIndex = 1,  // Default to "All Files"
            CheckFileExists = true
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtSavePath.Text = dialog.FileName;
            Log($"Selected save file: {dialog.FileName}");
        }
    }

    private void BtnBrowseOutput_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select Output Directory"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtOutputDir.Text = dialog.SelectedPath;
            Log($"Output directory: {dialog.SelectedPath}");
        }
    }

    private async void BtnDump_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtSavePath.Text) || !File.Exists(txtSavePath.Text))
        {
            MessageBox.Show("Please select a valid save file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(txtOutputDir.Text))
        {
            MessageBox.Show("Please select an output directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        btnDump.Enabled = false;
        progressBar.Value = 0;

        try
        {
            await Task.Run(() => ProcessSaveFile(txtSavePath.Text, txtOutputDir.Text));
            MessageBox.Show($"Save file dumped successfully!\n\nOutput: {txtOutputDir.Text}",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log($"ERROR: {ex.Message}");
            Log(ex.StackTrace ?? "");
            MessageBox.Show($"Failed to process save file:\n{ex.Message}",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnDump.Enabled = true;
            progressBar.Value = 0;
        }
    }

    private void ProcessSaveFile(string savePath, string outputDir)
    {
        Log("================================================================================");
        Log("Starting save file decryption and dump...");
        Log($"Save file: {savePath}");
        Log($"Output directory: {outputDir}");

        // Create timestamped output directory
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var dumpDir = Path.Combine(outputDir, $"dump_{timestamp}");
        Directory.CreateDirectory(dumpDir);
        Log($"Created dump directory: {dumpDir}");

        // Read save file
        Log("Reading save file...");
        var saveData = File.ReadAllBytes(savePath);
        Log($"Read {saveData.Length:N0} bytes ({saveData.Length / 1024.0:F2} KB)");

        UpdateProgress(10);

        // Validate hash
        Log("Validating save file hash...");
        var isValid = SwishCrypto.GetIsHashValid(saveData);
        Log(isValid ? "Hash validation: PASSED" : "Hash validation: FAILED (save may be corrupted)");

        if (!isValid)
        {
            var result = MessageBox.Show("Hash validation failed. Continue anyway?",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
                return;
        }

        UpdateProgress(20);

        // Decrypt using SwishCrypto
        Log("Decrypting save file with SwishCrypto.Decrypt()...");
        var blocks = SwishCrypto.Decrypt(saveData);
        Log($"Successfully decrypted {blocks.Count} blocks");

        UpdateProgress(30);

        // Try to create SAV9ZA instance for structured data
        SAV9ZA? sav = null;
        try
        {
            Log("Creating SAV9ZA instance for structured data parsing...");
            sav = new SAV9ZA(saveData);
            Log($"Save version: {sav.Version}");
            Log($"OT: {sav.OT}");
            Log($"TID: {sav.TrainerTID7:D6}");
            Log($"SID: {sav.TrainerSID7:D4}");
        }
        catch (Exception ex)
        {
            Log($"WARNING: Could not create SAV9ZA instance - {ex.Message}");
            Log("This may be due to a newer save version. Continuing with raw block dump...");
            Log("Structured data (party/boxes/items) will not be available.");
        }

        UpdateProgress(40);

        // Dump all blocks (this works even without SAV9ZA)
        Log("================================================================================");
        Log("Dumping all blocks...");
        DumpBlocks(blocks, dumpDir);

        UpdateProgress(60);

        // Dump block index
        Log("Creating block index...");
        CreateBlockIndex(blocks, dumpDir);

        UpdateProgress(70);

        // Only dump structured data if SAV9ZA was created successfully
        if (sav != null)
        {
            // Dump save metadata
            Log("Dumping save metadata...");
            DumpSaveMetadata(sav, dumpDir);

            UpdateProgress(80);

            // Dump structured data
            Log("Dumping structured data...");
            DumpStructuredData(sav, dumpDir);

            UpdateProgress(90);
        }
        else
        {
            Log("Skipping structured data export (SAV9ZA instance not available)");
            UpdateProgress(90);
        }

        UpdateProgress(100);

        Log("================================================================================");
        Log($"Dump complete! Output saved to: {dumpDir}");
        Log("================================================================================");

        // Open output directory
        System.Diagnostics.Process.Start("explorer.exe", dumpDir);
    }

    private void DumpBlocks(IReadOnlyList<SCBlock> blocks, string dumpDir)
    {
        var blocksDir = Path.Combine(dumpDir, "blocks");
        Directory.CreateDirectory(blocksDir);

        int unknownCount = 0;
        var unknownBlocks = new List<object>();

        for (int i = 0; i < blocks.Count; i++)
        {
            var block = blocks[i];
            var key = block.Key;
            var blockName = BlockNames.GetName(key);
            var isUnknown = blockName.StartsWith("Unknown_");

            if (isUnknown)
                unknownCount++;

            // Create filename
            var filename = $"{key:X8}_{blockName}.bin";
            var filepath = Path.Combine(blocksDir, filename);

            // Write binary data
            File.WriteAllBytes(filepath, block.Data.ToArray());

            // Analyze block data for hints
            var data = block.Data.ToArray();
            var hints = AnalyzeBlockData(data);

            // Write metadata
            var metaPath = filepath.Replace(".bin", ".meta.json");
            var meta = new
            {
                key = $"0x{key:X8}",
                key_decimal = key,
                name = blockName,
                is_unknown = isUnknown,
                type = block.Type.ToString(),
                subtype = block.Type == SCTypeCode.Array ? block.SubType.ToString() : null,
                size = block.Data.Length,
                size_hex = $"0x{block.Data.Length:X}",
                analysis_hints = isUnknown ? hints : null
            };

            var json = JsonSerializer.Serialize(meta, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(metaPath, json);

            // Track unknown blocks for summary
            if (isUnknown)
            {
                unknownBlocks.Add(new
                {
                    key = $"0x{key:X8}",
                    key_decimal = key,
                    size = data.Length,
                    size_hex = $"0x{data.Length:X}",
                    type = block.Type.ToString(),
                    hints
                });
            }

            if (i % 10 == 0)
                Log($"Dumped {i}/{blocks.Count} blocks...");
        }

        Log($"Dumped {blocks.Count} blocks to {blocksDir}");

        if (unknownCount > 0)
        {
            Log($"Found {unknownCount} unknown blocks - see unknown_blocks_summary.json for analysis hints");

            // Create unknown blocks summary
            var summaryPath = Path.Combine(dumpDir, "unknown_blocks_summary.json");
            var summary = new
            {
                total_unknown = unknownCount,
                total_blocks = blocks.Count,
                note = "These blocks are not yet identified. Compare with other saves or check PKHeX updates.",
                how_to_help = "If you figure out what these blocks do, please contribute to PKHeX!",
                blocks = unknownBlocks
            };

            var summaryJson = JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(summaryPath, summaryJson);
        }
    }

    private Dictionary<string, object> AnalyzeBlockData(byte[] data)
    {
        var hints = new Dictionary<string, object>();

        if (data.Length == 0)
        {
            hints["note"] = "Empty block";
            return hints;
        }

        // Check if mostly zeros
        var zeroCount = data.Count(b => b == 0);
        var zeroPercent = (zeroCount * 100.0) / data.Length;

        if (zeroPercent > 90)
            hints["mostly_zeros"] = $"{zeroPercent:F1}% zeros (possibly unused/cleared data)";

        // Check if mostly 0xFF
        var ffCount = data.Count(b => b == 0xFF);
        var ffPercent = (ffCount * 100.0) / data.Length;

        if (ffPercent > 90)
            hints["mostly_ff"] = $"{ffPercent:F1}% 0xFF bytes (possibly uninitialized)";

        // Check for repeating patterns
        if (data.Length >= 4)
        {
            var first4 = data.Take(4).ToArray();
            var isRepeating = true;
            for (int i = 4; i < Math.Min(data.Length, 32); i += 4)
            {
                if (!data.Skip(i).Take(4).SequenceEqual(first4))
                {
                    isRepeating = false;
                    break;
                }
            }
            if (isRepeating && data.Length >= 16)
                hints["pattern"] = $"Repeating 4-byte pattern: {BitConverter.ToString(first4)}";
        }

        // Check if looks like text (ASCII/UTF-8)
        var printableCount = data.Count(b => b >= 32 && b < 127);
        var printablePercent = (printableCount * 100.0) / data.Length;

        if (printablePercent > 50)
            hints["possible_text"] = $"{printablePercent:F1}% printable ASCII characters";

        // Show first 16 bytes as hex for manual inspection
        var preview = data.Take(Math.Min(16, data.Length)).ToArray();
        hints["first_bytes_hex"] = BitConverter.ToString(preview);

        // Size-based hints
        if (data.Length == 4)
            hints["possible_type"] = "Might be a single u32/i32/float value";
        else if (data.Length == 8)
            hints["possible_type"] = "Might be a u64/i64/double or two u32 values";
        else if (data.Length % 4 == 0)
            hints["size_note"] = $"Size is divisible by 4 ({data.Length / 4} potential u32 values)";

        return hints;
    }

    private void CreateBlockIndex(IReadOnlyList<SCBlock> blocks, string dumpDir)
    {
        var index = new List<object>();
        int unknownCount = 0;

        for (int i = 0; i < blocks.Count; i++)
        {
            var block = blocks[i];
            var key = block.Key;
            var blockName = BlockNames.GetName(key);
            var isUnknown = blockName.StartsWith("Unknown_");

            if (isUnknown)
                unknownCount++;

            var entry = new
            {
                index = i,
                key = $"0x{key:X8}",
                key_decimal = key,
                name = blockName,
                is_unknown = isUnknown,
                type = block.Type.ToString(),
                subtype = block.Type == SCTypeCode.Array ? block.SubType.ToString() : null,
                size = block.Data.Length,
                size_hex = $"0x{block.Data.Length:X}"
            };
            index.Add(entry);
        }

        // Save as JSON
        var indexPath = Path.Combine(dumpDir, "block_index.json");
        var json = JsonSerializer.Serialize(index, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(indexPath, json);
        Log($"Block index saved to {indexPath}");

        // Also save as CSV (with Unknown column for easy filtering in Excel)
        var csvPath = Path.Combine(dumpDir, "block_index.csv");
        using var writer = new StreamWriter(csvPath);
        writer.WriteLine("Index,Key (Hex),Key (Dec),Name,Unknown,Type,Subtype,Size,Size (Hex)");
        for (int i = 0; i < blocks.Count; i++)
        {
            var block = blocks[i];
            var key = block.Key;
            var blockName = BlockNames.GetName(key);
            var isUnknown = blockName.StartsWith("Unknown_") ? "YES" : "NO";
            var subtype = block.Type == SCTypeCode.Array ? block.SubType.ToString() : "";
            writer.WriteLine($"{i},0x{key:X8},{key},{blockName},{isUnknown},{block.Type},{subtype},{block.Data.Length},0x{block.Data.Length:X}");
        }
        Log($"Block index CSV saved to {csvPath}");

        if (unknownCount > 0)
        {
            Log($"NOTE: {unknownCount} blocks are unknown - filter by 'Unknown=YES' in the CSV to see them");
        }
    }

    private void DumpSaveMetadata(SAV9ZA sav, string dumpDir)
    {
        var metadata = new
        {
            version = sav.Version.ToString(),
            ot_name = sav.OT,
            tid7 = sav.TrainerTID7,
            sid7 = sav.TrainerSID7,
            gender = sav.Gender == 0 ? "Male" : "Female",
            language = sav.Language.ToString(),
            game_version = ((int)sav.Version).ToString(),
            save_revision = sav.SaveRevision,
            boxes_unlocked = sav.BoxesUnlocked,
            box_count = 32,
            play_time = new
            {
                hours = sav.PlayedHours,
                minutes = sav.PlayedMinutes,
                seconds = sav.PlayedSeconds
            }
        };

        var metaPath = Path.Combine(dumpDir, "save_metadata.json");
        var json = JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(metaPath, json);
        Log($"Save metadata saved to {metaPath}");
    }

    private void DumpStructuredData(SAV9ZA sav, string dumpDir)
    {
        var structuredDir = Path.Combine(dumpDir, "structured");
        Directory.CreateDirectory(structuredDir);

        // Dump party
        Log("Dumping party data...");
        var partyDir = Path.Combine(structuredDir, "party");
        Directory.CreateDirectory(partyDir);

        for (int i = 0; i < sav.PartyCount && i < 6; i++)
        {
            var pk = sav.GetPartySlotAtIndex(i);
            if (pk.Species != 0)
            {
                var partyData = new
                {
                    slot = i,
                    species = pk.Species,
                    nickname = pk.Nickname,
                    level = pk.CurrentLevel,
                    pid = $"0x{pk.PID:X8}",
                    is_shiny = pk.IsShiny,
                    nature = pk.Nature.ToString(),
                    ability = pk.Ability,
                    gender = pk.Gender.ToString(),
                    current_hp = pk.Stat_HPCurrent,
                    max_hp = pk.Stat_HPMax,
                    atk = pk.Stat_ATK,
                    def = pk.Stat_DEF,
                    spa = pk.Stat_SPA,
                    spd = pk.Stat_SPD,
                    spe = pk.Stat_SPE,
                    moves = new[] { pk.GetMove(0), pk.GetMove(1), pk.GetMove(2), pk.GetMove(3) }
                };

                var partyPath = Path.Combine(partyDir, $"slot_{i}_{pk.Nickname}.json");
                var json = JsonSerializer.Serialize(partyData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(partyPath, json, Encoding.UTF8);
            }
        }

        // Dump box summary
        Log("Dumping box data summary...");
        var boxSummary = new List<object>();
        for (int box = 0; box < 32; box++)
        {
            int count = 0;
            for (int slot = 0; slot < 30; slot++)
            {
                var pk = sav.GetBoxSlotAtIndex(box * 30 + slot);
                if (pk.Species != 0)
                    count++;
            }
            boxSummary.Add(new
            {
                box = box,
                name = sav.GetBoxName(box),
                pokemon_count = count
            });
        }

        var boxPath = Path.Combine(structuredDir, "box_summary.json");
        var boxJson = JsonSerializer.Serialize(boxSummary, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(boxPath, boxJson, Encoding.UTF8);

        // Dump items
        Log("Dumping items...");
        var itemsData = new List<object>();
        for (ushort i = 0; i < MyItem9a.ItemSaveSize; i++)
        {
            var item = sav.Items.GetItem(i);
            if (item.Count > 0)
            {
                itemsData.Add(new
                {
                    index = item.Index,
                    count = item.Count
                });
            }
        }

        var itemsPath = Path.Combine(structuredDir, "items.json");
        var itemsJson = JsonSerializer.Serialize(itemsData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(itemsPath, itemsJson);

        Log($"Structured data saved to {structuredDir}");
    }

    private void Log(string message)
    {
        if (txtLog.InvokeRequired)
        {
            txtLog.Invoke(() => Log(message));
            return;
        }

        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        txtLog.AppendText($"[{timestamp}] {message}\n");
        txtLog.ScrollToCaret();
    }

    private void UpdateProgress(int value)
    {
        if (progressBar.InvokeRequired)
        {
            progressBar.Invoke(() => progressBar.Value = value);
        }
        else
        {
            progressBar.Value = value;
        }
    }
}
