﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sledge.Editor
{
    public enum EditorMediator
    {
        // Settings messages
        SettingsChanged,
        OpenSettings,

        // Document messages
        DocumentOpened,
        DocumentSaved,
        DocumentDeactivated,
        DocumentActivated,
        DocumentClosed,
        DocumentAllClosed,

        // Document manager messages

        // Editing messages
        TextureSelected,
        ToolSelected,
        ViewportRightClick,
        WorldspawnProperties,
        ResetSelectedBrushType,
        SetZoomValue,

        VisgroupToggled,
        VisgroupsChanged,
        VisgroupVisibilityChanged,
        VisgroupShowEditor,
        VisgroupSelect,
        VisgroupShowAll,

        // Action messages

        DocumentTreeStructureChanged,
        DocumentTreeObjectsChanged,
        DocumentTreeFacesChanged,

        EntityDataChanged,

        SelectionTypeChanged,
        SelectionChanged,

        HistoryChanged,
        ClipboardChanged,

        // Status bar messages
        MouseCoordinatesChanged,
        SelectionBoxChanged,
        ViewZoomChanged,
        ViewFocused,
        ViewUnfocused,
        DocumentGridSpacingChanged,

        // File system messages
        FileOpened,
        FileSaved,
        FileAutosaved,

        // Editor messages
        LoadFile,
        UpdateMenu,
        UpdateToolstrip,
        CheckForUpdates,
        OpenWebsite,
        About,
        Exit
    }
}
