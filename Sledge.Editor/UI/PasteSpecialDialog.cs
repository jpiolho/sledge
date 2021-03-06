﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sledge.DataStructures.Geometric;
using Sledge.Editor.Enums;

namespace Sledge.Editor.UI
{
    public partial class PasteSpecialDialog : Form
    {
        private readonly Box _source;

        public int NumberOfCopies
        {
            get { return (int) NumCopies.Value; }
            set { NumCopies.Value = value; }
        }

        public PasteSpecialStartPoint StartPoint
        {
            get
            {
                if (StartOrigin.Checked) return PasteSpecialStartPoint.Origin;
                if (StartOriginal.Checked) return PasteSpecialStartPoint.CenterOriginal;
                return PasteSpecialStartPoint.CenterSelection;
            }
            set
            {
                switch (value)
                {
                    case PasteSpecialStartPoint.Origin:
                        StartOrigin.Checked = true;
                        break;
                    case PasteSpecialStartPoint.CenterOriginal:
                        StartOriginal.Checked = true;
                        break;
                    case PasteSpecialStartPoint.CenterSelection:
                        StartSelection.Checked = true;
                        break;
                }
            }
        }

        public PasteSpecialGrouping Grouping
        {
            get
            {
                if (GroupNone.Checked) return PasteSpecialGrouping.None;
                if (GroupIndividual.Checked) return PasteSpecialGrouping.Individual;
                return PasteSpecialGrouping.All;
            }
            set
            {
                switch (value)
                {
                    case PasteSpecialGrouping.None:
                        GroupNone.Checked = true;
                        break;
                    case PasteSpecialGrouping.Individual:
                        GroupIndividual.Checked = true;
                        break;
                    case PasteSpecialGrouping.All:
                        GroupAll.Checked = true;
                        break;
                }
            }
        }

        public Coordinate AccumulativeOffset
        {
            get { return new Coordinate(OffsetX.Value, OffsetY.Value, OffsetZ.Value); }
            set
            {
                OffsetX.Value = value.X;
                OffsetY.Value = value.Y;
                OffsetZ.Value = value.Z;
            }
        }

        public Coordinate AccumulativeRotation
        {
            get { return new Coordinate(RotationX.Value, RotationY.Value, RotationZ.Value); }
            set
            {
                RotationX.Value = value.X;
                RotationY.Value = value.Y;
                RotationZ.Value = value.Z;
            }
        }

        public bool MakeEntitiesUnique
        {
            get { return UniqueEntityNames.Checked; }
            set { UniqueEntityNames.Checked = value; }
        }

        public bool PrefixEntityNames
        {
            get { return PrefixEntityNamesCheckbox.Checked; }
            set
            {
                PrefixEntityNamesCheckbox.Checked = value;
                EntityPrefix.Enabled = PrefixEntityNamesCheckbox.Checked;
            }
        }

        public string EntityNamePrefix
        {
            get { return EntityPrefix.Text; }
            set { EntityPrefix.Text = value; }
        }

        public PasteSpecialDialog(Box source)
        {
            _source = source;
            InitializeComponent();
            EntityPrefix.Enabled = PrefixEntityNamesCheckbox.Checked;

            ZeroOffsetXButton.Click += (sender, e) => OffsetX.Value = 0;
            ZeroOffsetYButton.Click += (sender, e) => OffsetY.Value = 0;
            ZeroOffsetZButton.Click += (sender, e) => OffsetZ.Value = 0;

            SourceOffsetXButton.Click += (sender, e) => OffsetX.Value = _source.Width;
            SourceOffsetYButton.Click += (sender, e) => OffsetY.Value = _source.Length;
            SourceOffsetZButton.Click += (sender, e) => OffsetZ.Value = _source.Height;

            ZeroRotationXButton.Click += (sender, e) => RotationX.Value = 0;
            ZeroRotationYButton.Click += (sender, e) => RotationY.Value = 0;
            ZeroRotationZButton.Click += (sender, e) => RotationZ.Value = 0;

            PrefixEntityNamesCheckbox.CheckedChanged += (sender, e) => EntityPrefix.Enabled = PrefixEntityNamesCheckbox.Checked;
        }
    }
}
