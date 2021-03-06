﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sledge.Settings;

namespace Sledge.Editor.Tools.DisplacementTools
{
    public abstract class DisplacementSubTool : BaseTool
    {
        public Control Control { get; set; }

        public override Image GetIcon()
        {
            throw new NotImplementedException();
        }

        public override HotkeyTool? GetHotkeyToolType()
        {
            return null;
        }

        public override HotkeyInterceptResult InterceptHotkey(HotkeysMediator hotkeyMessage)
        {
            switch (hotkeyMessage)
            {
                case HotkeysMediator.OperationsCopy:
                case HotkeysMediator.OperationsCut:
                case HotkeysMediator.OperationsPaste:
                case HotkeysMediator.OperationsPasteSpecial:
                case HotkeysMediator.OperationsDelete:
                    return HotkeyInterceptResult.Abort;
            }
            return HotkeyInterceptResult.Continue;
        }
    }
}
