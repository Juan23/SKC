using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SKC_Admin
{
    public partial class frmRecipeEditor : Form
    {
        private readonly List<AdminProduct> catalog;
        private readonly int? editingRecipeId;
        private readonly BindingList<DraftLine> draftLines = new BindingList<DraftLine>();

        // Create mode
        public frmRecipeEditor(List<AdminProduct> catalog)
        {
            this.catalog = catalog;
            InitializeComponent();
            SetupCombos();
            Text = "New Recipe";
        }

        // Edit mode - seeds every field (including lines) from the existing recipe.
        public frmRecipeEditor(Recipe existing, List<AdminProduct> catalog) : this(catalog)
        {
            editingRecipeId = existing.RecipeId;
            txtName.Text = existing.Name;
            cmbKind.SelectedItem = existing.Kind;
            cmbOutputSku.SelectedValue = existing.OutputSku;
            numOutputQty.Value = existing.OutputQty;

            foreach (var line in existing.Lines)
            {
                var product = catalog.FirstOrDefault(p => p.SKU == line.InputSku);
                draftLines.Add(new DraftLine
                {
                    InputSku = line.InputSku,
                    ItemName = product?.Display ?? line.InputSku,
                    Qty = line.Qty
                });
            }

            Text = "Edit Recipe";
        }

        private void SetupCombos()
        {
            cmbKind.Items.AddRange(new object[] { "Baking", "Decorating" });

            // Output must be something production creates, never a raw material.
            cmbOutputSku.DataSource = catalog.Where(p => p.Category != "RawMaterial").ToList();
            cmbOutputSku.DisplayMember = "Display";
            cmbOutputSku.ValueMember = "SKU";

            // Inputs can be anything (raw materials, or a BakedGood for decorating recipes).
            cmbInputSku.DataSource = catalog.ToList();
            cmbInputSku.DisplayMember = "Display";
            cmbInputSku.ValueMember = "SKU";

            dgvLines.DataSource = draftLines;
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            if (cmbInputSku.SelectedItem is not AdminProduct selected || numLineQty.Value <= 0) return;

            draftLines.Add(new DraftLine
            {
                InputSku = selected.SKU,
                ItemName = selected.Display,
                Qty = (int)numLineQty.Value
            });
            numLineQty.Value = 1;
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvLines.CurrentRow?.DataBoundItem is DraftLine selected)
                draftLines.Remove(selected);
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter a recipe name.", "Missing Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbKind.SelectedItem == null)
            {
                MessageBox.Show("Select Baking or Decorating.", "Missing Kind", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbOutputSku.SelectedValue == null)
            {
                MessageBox.Show("Select an output product.", "Missing Output", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (draftLines.Count == 0)
            {
                MessageBox.Show("Add at least one input line.", "No Input Lines", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var recipe = new Recipe
            {
                Name = txtName.Text.Trim(),
                Kind = cmbKind.SelectedItem.ToString()!,
                OutputSku = cmbOutputSku.SelectedValue.ToString()!,
                OutputQty = (int)numOutputQty.Value,
                Lines = draftLines.Select(l => new RecipeLine { InputSku = l.InputSku, Qty = l.Qty }).ToList()
            };

            try
            {
                if (editingRecipeId.HasValue)
                    await AdminApiClient.UpdateRecipeAsync(editingRecipeId.Value, recipe);
                else
                    await AdminApiClient.CreateRecipeAsync(recipe);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class DraftLine
        {
            public string InputSku { get; set; } = string.Empty;

            [DisplayName("Item")]
            public string ItemName { get; set; } = string.Empty;

            [DisplayName("Qty")]
            public int Qty { get; set; }
        }
    }
}
