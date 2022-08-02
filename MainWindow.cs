using GLib;
using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmapGui
{
    public class MainWindow : Window
    {
        private MenuBar MainMenuBar;
        private Menu FileMenu;
        private Menu ModeMenu;
        private Menu EditMenu;
        private Menu GridMenu;

        private MenuItem FileMenuItem;
        private MenuItem LoadMenuItem;
        private MenuItem SaveMenuItem;
        private MenuItem ReloadMenuItem;
        private MenuItem EditMenuItem;
        private MenuItem GridMenuItem;
        private MenuItem Grid4MenuItem;
        private MenuItem Grid8MenuItem;
        private MenuItem Grid16MenuItem;
        private MenuItem ModeMenuItem;
        private CheckMenuItem SMBXWorldModeMenuItem;
        private CheckMenuItem XMapWorldModeMenuItem;

        private Frame ToolboxFrame;
        private Frame CameraFrame;
        private DrawingArea CamToolbox;
        private Frame SPPathFrame;
        private DrawingArea PathToolbox;
        private Frame AnimeFrame;
        private DrawingArea AnimeToolbox;
        private Frame CoinSignFrame;
        private DrawingArea CoinSignToolbox;

        private DrawingArea WorldScene;

        private Notebook ActionNotebook;
        private VBox ItemPropertiesContainer;
        private Label ItemPropertyMainLabel;
        private Label ItemInfoLabel;
        private Label EventLabel;
        private ComboBox EventComboBox;
        private Frame ItemConfigFrame;
        private Entry ItemWidthEntry;
        private Entry ItemHeightEntry;
        private Entry ItemFramesEntry;
        private Entry ItemFrameSpeedEntry;
        private Entry ItemPriorityEntry;
        private Entry ItemPFramesEntry;
        private Button ItemConfigSaveButton;
        private Frame WAnimeInstConfigFrame;
        private Entry WAnimeSpeedXEntry;
        private Entry WAnimeSpeedYEntry;
        private Entry WAnimeAccelXEntry;
        private Entry WAnimeAccelYEntry;
        private Entry WAnimeLifetimeEntry;
        private Entry WAnimeStartFrameEntry;
        private Entry CamWidthEntry;
        private Entry CamHeightEntry;
        private Entry SPathWaitEntry;
        private Entry SignCoinsEntry;

        private VBox EventsContainer;
        private ScrolledWindow EventContainer;
        private TreeView EventView;
        private ScrolledWindow EventActionContainer;
        private TreeView EventActionView;
        private ListStore EventActions;
        private Button AddEventButton;
        private Button RemoveEventButton;
        private Button RemoveActionButton;
        private Button AddTileActionButton;
        private Button AddPathActionButton;
        private Button AddMusicActionButton;
        private Button AddPlayerActionButton;
        private Button AddCamActionButton;
        private Button AddAnimeActionButton;

        private ScrolledWindow ActionsContainer;
        private Frame AddTileActionFrame;
        private RadioButton TileActionTranslateRadioButton;
        private RadioButton TileActionSetIDRadioButton;
        private RadioButton TileActionSwapRadioButton;
        private Entry[] TileActionParamEntries = new Entry[6];
        private CheckButton TileActionSaveCheckButton;
        private Frame AddPathActionFrame;
        private Entry PathActionXEntry;
        private Entry PathActionYEntry;
        private Frame AddMusicActionFrame;
        private SpinButton MusicActionSpinButton;
        private Frame AddPlayerActionFrame;
        private CheckButton PlayerActionCheckBox;
        private Frame AddCamActionFrame;
        private Entry CamActionXEntry;
        private Entry CamActionYEntry;
        private Entry CamActionWEntry;
        private Entry CamActionHEntry;
        private Frame AddAnimeActionFrame;
        private Entry AnimeActionIDEntry;
        private Entry AnimeActionXEntry;
        private Entry AnimeActionYEntry;
        private Entry AnimeActionSpeedXEntry;
        private Entry AnimeActionSpeedYEntry;
        private Entry AnimeActionAccelXEntry;
        private Entry AnimeActionAccelYEntry;
        private Entry AnimeActionLifetimeEntry;
        private Entry AnimeActionStartFrameEntry;

        private VBox TriggersContainer;
        private RadioButton PathTriggerRadioButton;
        private RadioButton LevelTriggerRadioButton;
        private ComboBox BindEventComboBox;
        private Button AddTriggerButton;
        private Button RemoveTriggerButton;
        private ScrolledWindow TriggerViewContainer;
        private TreeView TriggerView;
        private ScrolledWindow TriggerExprViewContainer;
        private TreeView TriggerExpressionView;
        private ListStore TriggerExpressions;
        private Button AddTriggerCondCButton;
        private Button AddTriggerCondDButton;
        private Button ClearTriggerCondButton;
        private RadioButton TriggerXPosRadioButton;
        private RadioButton TriggerYPosRadioButton;
        private RadioButton TriggerEqRadioButton;
        private RadioButton TriggerNeqRadioButton;
        private RadioButton TriggerGrRadioButton;
        private RadioButton TriggerLsRadioButton;
        private Entry TriggerConditionCompareEntry;
        private ScrolledWindow TriggerExpressionContainer;
        private Entry[] TriggerExpressionEntries = new Entry[9];
        private Button AddTriggerExpressionButton;
        private Button RemoveTriggerExpressionButton;

        private VBox MainContainer;
        private HBox MainSubContainer;

        private FileFilter WLDFilter;

        private bool Drawing = false;
        private bool Dragging = false;
        private int DragOffsetX = 0;
        private int DragOffsetY = 0;
        private bool Placing = false;
        private int PlaceType = 0;
        private ushort PlaceTypeID = 0;
        private int PlaceX = 0;
        private int PlaceY = 0;
        private int CurrentEventIndex = 0;
        private int CurrentEventActionIndex = -1;

        private int GridX = 16;
        private int GridY = 16;

        private int ViewW = 1120;
        private int ViewH = 800;

        private int ViewX = -560;
        private int ViewY = -400;

        private const int KEY_UP = 0x01;
        private const int KEY_LEFT = 0x02;
        private const int KEY_DOWN = 0x04;
        private const int KEY_RIGHT = 0x08;

        private const int PLACE_TYPE_ANIME = 1;
        private const int PLACE_TYPE_CAM = 2;
        private const int PLACE_TYPE_SIGN = 3;
        private const int PLACE_TYPE_SPATH = 4;

        private int Keys = 0;

        private bool KeyPressing(int K)
        {
            return (Keys & K) != 0;
        }

        public MainWindow(WindowType WType, string Title) : base(WType)
        {
            this.Title = Title;
            Resizable = false;

            DeleteEvent += _OnClose;

            InitWidgets();
        }

        private void InitWidgets()
        {
            MainContainer = new VBox(false, 16);
            MainSubContainer = new HBox(false, 16);
            ItemPropertiesContainer = new VBox(false, 16);
            EventsContainer = new VBox(false, 16);
            TriggersContainer = new VBox(false, 16);

            MainMenuBar = new MenuBar();
            FileMenu = new Menu();
            EditMenu = new Menu();
            GridMenu = new Menu();
            ModeMenu = new Menu();

            FileMenuItem = new MenuItem("File");
            LoadMenuItem = new MenuItem("Open");
            SaveMenuItem = new MenuItem("Save");
            SaveMenuItem.Sensitive = false;
            ReloadMenuItem = new MenuItem("Reload Current World");
            ReloadMenuItem.Sensitive = false;
            EditMenuItem = new MenuItem("Edit");
            GridMenuItem = new MenuItem("Set Grid Alignment");
            Grid4MenuItem = new MenuItem("4x4");
            Grid8MenuItem = new MenuItem("8x8");
            Grid16MenuItem = new MenuItem("16x16");
            ModeMenuItem = new MenuItem("Mode");
            ModeMenuItem.Sensitive = false;
            SMBXWorldModeMenuItem = new CheckMenuItem("Vanilla Item Selection Mode");
            XMapWorldModeMenuItem = new CheckMenuItem("XMap Item Selection Mode");
            SMBXWorldModeMenuItem.Active = true;

            FileMenu.Append(LoadMenuItem);
            FileMenu.Append(SaveMenuItem);
            FileMenu.Append(new SeparatorMenuItem());
            FileMenu.Append(ReloadMenuItem);
            EditMenu.Append(GridMenuItem);
            GridMenu.Append(Grid4MenuItem);
            GridMenu.Append(Grid8MenuItem);
            GridMenu.Append(Grid16MenuItem);
            ModeMenu.Append(SMBXWorldModeMenuItem);
            ModeMenu.Append(XMapWorldModeMenuItem);
            FileMenuItem.Submenu = FileMenu;
            ModeMenuItem.Submenu = ModeMenu;
            EditMenuItem.Submenu = EditMenu;
            GridMenuItem.Submenu = GridMenu;

            MainMenuBar.Append(FileMenuItem);
            MainMenuBar.Append(EditMenuItem);
            MainMenuBar.Append(ModeMenuItem);

            ToolboxFrame = new Frame("Items");
            ToolboxFrame.WidthRequest = 256;
            ToolboxFrame.Sensitive = false;
            CameraFrame = new Frame("Camera");
            SPPathFrame = new Frame("Special Path Markers");
            AnimeFrame = new Frame("Animations");
            CoinSignFrame = new Frame("Star Coin Signs");
            AnimeToolbox = new DrawingArea();
            CamToolbox = new DrawingArea();
            PathToolbox = new DrawingArea();
            CoinSignToolbox = new DrawingArea();
            AnimeFrame.Add(GuiUtils.CompactPack(AnimeToolbox, true, true, false, true, 4, 4));
            CameraFrame.Add(GuiUtils.CompactPack(CamToolbox, true, true, false, true, 4, 4));
            SPPathFrame.Add(GuiUtils.CompactPack(PathToolbox, true, true, false, true, 4, 4));
            CoinSignFrame.Add(GuiUtils.CompactPack(CoinSignToolbox, true, true, false, true, 4, 4));
            AnimeToolbox.WidthRequest = 224;
            AnimeToolbox.AddEvents((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask));
            CamToolbox.WidthRequest = 224;
            CamToolbox.AddEvents((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask));
            PathToolbox.WidthRequest = 224;
            PathToolbox.AddEvents((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask));
            CoinSignToolbox.WidthRequest = 224;
            CoinSignToolbox.AddEvents((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask));

            ToolboxFrame.Add(GuiUtils.CompactPackMultiV(false, false, 4, true, true, false, false, 4, 4,
                CameraFrame,
                SPPathFrame,
                AnimeFrame,
                CoinSignFrame
                ));

            WorldScene = new DrawingArea();
            WorldScene.SetSizeRequest(ViewW, ViewH);
            WorldScene.AddEvents((int)(Gdk.EventMask.ButtonPressMask | Gdk.EventMask.ButtonReleaseMask | Gdk.EventMask.PointerMotionMask));

            ActionNotebook = new Notebook();
            ActionNotebook.AppendPage(ItemPropertiesContainer, new Label("Item Properties"));
            ActionNotebook.AppendPage(EventsContainer, new Label("Events"));
            ActionNotebook.AppendPage(TriggersContainer, new Label("Triggers"));
            ActionNotebook.WidthRequest = 256;
            ActionNotebook.Sensitive = false;

            ItemPropertyMainLabel = new Label();
            ItemInfoLabel = new Label();

            ItemPropertyMainLabel.UseMarkup = true;
            ItemPropertyMainLabel.Markup = "<span size = \"x-large\" weight = \"heavy\">Item Information:</span>";

            EventLabel = new Label("Event when revealed: ");
            EventComboBox = new ComboBox(ProgramState.EventStore);
            CellRendererText _ComboBoxRenderer = new CellRendererText();
            EventComboBox.PackStart(_ComboBoxRenderer, false);
            EventComboBox.AddAttribute(_ComboBoxRenderer, "text", 0);

            int TextBoxWidth = 136;
            int TextBoxWidthL = 148;
            ItemConfigFrame = new Frame("Configuration (type-wide):");
            ItemWidthEntry = new Entry();
            ItemWidthEntry.WidthRequest = TextBoxWidth;
            ItemHeightEntry = new Entry();
            ItemHeightEntry.WidthRequest = TextBoxWidth;
            ItemFramesEntry = new Entry();
            ItemFramesEntry.WidthRequest = TextBoxWidth;
            ItemFrameSpeedEntry = new Entry();
            ItemFrameSpeedEntry.WidthRequest = TextBoxWidth;
            ItemPriorityEntry = new Entry();
            ItemPriorityEntry.WidthRequest = TextBoxWidth;
            ItemPFramesEntry = new Entry();
            ItemPFramesEntry.WidthRequest = TextBoxWidth;
            ItemConfigSaveButton = new Button("Save to file");
            ItemConfigFrame.Add(GuiUtils.CompactPackMultiV(false, false, 2, true, true, false, false, 2, 0,
                GuiUtils.CompactPackMultiH(false, false, 2, false, false, true, true, 0, 0,
                    GuiUtils.CompactPackMultiV(true, false, 2, false, false, false, false, 0, 0,
                            new Label("Width: "),
                            new Label("Height: "),
                            new Label("Frames: "),
                            new Label("Framespeed: "),
                            new Label("Priority: "),
                            new Label("Frames (Pressed): ")
                        ),
                    GuiUtils.CompactPackMultiV(true, false, 2, false, false, false, false, 0, 0,
                            ItemWidthEntry,
                            ItemHeightEntry,
                            ItemFramesEntry,
                            ItemFrameSpeedEntry,
                            ItemPriorityEntry,
                            ItemPFramesEntry
                        )
                    ),
                GuiUtils.CompactPack(ItemConfigSaveButton, true, true, false, false, 4, 4)
                ));


            WAnimeInstConfigFrame = new Frame("Configuration (per-instance): ");
            WAnimeSpeedXEntry = new Entry();
            WAnimeSpeedXEntry.WidthRequest = TextBoxWidthL;
            WAnimeSpeedYEntry = new Entry();
            WAnimeSpeedYEntry.WidthRequest = TextBoxWidthL;
            WAnimeAccelXEntry = new Entry();
            WAnimeAccelXEntry.WidthRequest = TextBoxWidthL;
            WAnimeAccelYEntry = new Entry();
            WAnimeAccelYEntry.WidthRequest = TextBoxWidthL;
            WAnimeLifetimeEntry = new Entry();
            WAnimeLifetimeEntry.WidthRequest = TextBoxWidthL;
            WAnimeStartFrameEntry = new Entry();
            WAnimeStartFrameEntry.WidthRequest = TextBoxWidthL;
            CamWidthEntry = new Entry();
            CamWidthEntry.WidthRequest = TextBoxWidthL;
            CamHeightEntry = new Entry();
            CamHeightEntry.WidthRequest = TextBoxWidthL;
            SPathWaitEntry = new Entry();
            SPathWaitEntry.WidthRequest = TextBoxWidthL;
            SignCoinsEntry = new Entry();
            SignCoinsEntry.WidthRequest = TextBoxWidthL;
            WAnimeInstConfigFrame.Add(GuiUtils.CompactPack(GuiUtils.CompactPackMultiH(false, false, 2, false, false, true, true, 0, 0,
                    GuiUtils.CompactPackMultiV(true, false, 2, false, false, false, false, 0, 0,
                            new Label("X Speed: "),
                            new Label("Y Speed: "),
                            new Label("X Accel: "),
                            new Label("Y Accel: "),
                            new Label("Lifetime: "),
                            new Label("Starting frame: "),
                            new Label("Width: "),
                            new Label("Height: "),
                            new Label("Wait time: "),
                            new Label("Required coins: ")
                        ),
                    GuiUtils.CompactPackMultiV(true, false, 2, false, false, false, false, 0, 0,
                            WAnimeSpeedXEntry,
                            WAnimeSpeedYEntry,
                            WAnimeAccelXEntry,
                            WAnimeAccelYEntry,
                            WAnimeLifetimeEntry,
                            WAnimeStartFrameEntry,
                            CamWidthEntry,
                            CamHeightEntry,
                            SPathWaitEntry,
                            SignCoinsEntry
                        )
                    ), true, true, false, false, 2, 2));

            ItemPropertiesContainer.PackStart(GuiUtils.CompactPackMultiV(false, false, 4, false, true, false, false, 4, 4,
                ItemPropertyMainLabel,
                ItemInfoLabel,
                GuiUtils.CompactPackMultiH(false, true, 0, true, true, true, true, 0, 0,
                    EventLabel,
                    EventComboBox
                    ),
                ItemConfigFrame,
                WAnimeInstConfigFrame
                ));

            EventContainer = new ScrolledWindow();
            EventView = new TreeView(ProgramState.EventStore);
            EventView.AppendColumn("Event", new CellRendererText(), "text", 0);
            EventView.HeadersVisible = false;
            EventView.HeightRequest = 128;
            EventContainer.Add(EventView);
            EventActionContainer = new ScrolledWindow();
            EventActions = new ListStore(typeof(string));
            EventActionView = new TreeView(EventActions);
            EventActionView.HeadersVisible = false;
            EventActionView.HeightRequest = 128;
            EventActionView.AppendColumn("Action", new CellRendererText(), "text", 0);
            EventActionContainer.Add(EventActionView);
            AddEventButton = new Button("Add Event");
            RemoveEventButton = new Button("Remove Event");
            RemoveActionButton = new Button("Remove Action");

            ActionsContainer = new ScrolledWindow();
            ActionsContainer.VscrollbarPolicy = PolicyType.Automatic;
            ActionsContainer.BorderWidth = 0;
            ActionsContainer.HeightRequest = 420;

            AddTileActionButton = new Button("+");
            AddTileActionButton.WidthRequest = 24;
            AddTileActionButton.HeightRequest = 24;
            AddPathActionButton = new Button("+");
            AddPathActionButton.WidthRequest = 24;
            AddPathActionButton.HeightRequest = 24;
            AddMusicActionButton = new Button("+");
            AddMusicActionButton.WidthRequest = 24;
            AddMusicActionButton.HeightRequest = 24;
            AddPlayerActionButton = new Button("+");
            AddPlayerActionButton.WidthRequest = 24;
            AddPlayerActionButton.HeightRequest = 24;
            AddCamActionButton = new Button("+");
            AddCamActionButton.WidthRequest = 24;
            AddCamActionButton.HeightRequest = 24;
            AddAnimeActionButton = new Button("+");
            AddAnimeActionButton.WidthRequest = 24;
            AddAnimeActionButton.HeightRequest = 24;

            AddTileActionFrame = new Frame();
            AddTileActionFrame.Sensitive = false;
            AddTileActionFrame.LabelWidget = AddTileActionButton;
            AddPathActionFrame = new Frame();
            AddPathActionFrame.Sensitive = false;
            AddPathActionFrame.LabelWidget = AddPathActionButton;
            AddMusicActionFrame = new Frame();
            AddMusicActionFrame.Sensitive = false;
            AddMusicActionFrame.LabelWidget = AddMusicActionButton;
            AddPlayerActionFrame = new Frame();
            AddPlayerActionFrame.Sensitive = false;
            AddPlayerActionFrame.LabelWidget = AddPlayerActionButton;
            AddCamActionFrame = new Frame();
            AddCamActionFrame.Sensitive = false;
            AddCamActionFrame.LabelWidget = AddCamActionButton;
            AddAnimeActionFrame = new Frame();
            AddAnimeActionFrame.Sensitive = false;
            AddAnimeActionFrame.LabelWidget = AddAnimeActionButton;

            TileActionTranslateRadioButton = new RadioButton("Translate");
            TileActionSetIDRadioButton = new RadioButton(TileActionTranslateRadioButton, "Set ID");
            TileActionSwapRadioButton = new RadioButton(TileActionTranslateRadioButton, "Swap");

            for (int i = 0; i < 6; i++)
                TileActionParamEntries[i] = new Entry();

            TileActionSaveCheckButton = new CheckButton("Save");

            AddTileActionFrame.Add(GuiUtils.CompactPackMultiV(false, false, 2, false, true, false, false, 4, 0,
                new Label("XTile Operations"),
                TileActionTranslateRadioButton,
                TileActionSetIDRadioButton,
                TileActionSwapRadioButton,
                TileActionParamEntries[0],
                TileActionParamEntries[1],
                TileActionParamEntries[2],
                TileActionParamEntries[3],
                TileActionParamEntries[4],
                TileActionParamEntries[5],
                TileActionSaveCheckButton
                ));

            PathActionXEntry = new Entry();
            PathActionYEntry = new Entry();

            AddPathActionFrame.Add(GuiUtils.CompactPackMultiV(false, false, 2, false, true, false, false, 4, 0,
                new Label("Open path"),
                GuiUtils.CompactPackMultiH(false, false, 2, false, false, true, true, 0, 0,
                    GuiUtils.CompactPackMultiV(true, false, 2, false, false, false, false, 0, 0,
                            new Label("X: "),
                            new Label("Y: ")
                        ),
                    GuiUtils.CompactPackMultiV(true, false, 2, true, true, false, false, 0, 0,
                            PathActionXEntry,
                            PathActionYEntry
                        )
                    )));

            MusicActionSpinButton = new SpinButton(0, 16, 1);

            AddMusicActionFrame.Add(GuiUtils.CompactPackMultiV(false, false, 2, false, true, false, false, 4, 0,
                new Label("Play music"),
                GuiUtils.CompactPackMultiH(false, false, 2, false, false, true, true, 0, 0,
                    new Label("ID: "),
                    MusicActionSpinButton
                    )));

            PlayerActionCheckBox = new CheckButton("Set player visibility");

            AddPlayerActionFrame.Add(GuiUtils.CompactPack(PlayerActionCheckBox, true, true, false, false, 4, 4));

            CamActionXEntry = new Entry();
            CamActionYEntry = new Entry();
            CamActionWEntry = new Entry();
            CamActionHEntry = new Entry();

            AddCamActionFrame.Add(GuiUtils.CompactPackMultiV(false, false, 2, false, true, false, false, 4, 0,
                new Label("Add camera boundary"),
                GuiUtils.CompactPackMultiH(false, false, 2, false, false, true, true, 0, 0,
                    GuiUtils.CompactPackMultiV(true, false, 2, false, false, false, false, 0, 0,
                            new Label("X: "),
                            new Label("Y: "),
                            new Label("Width: "),
                            new Label("Height: ")
                        ),
                    GuiUtils.CompactPackMultiV(true, false, 2, true, true, false, false, 0, 0,
                            CamActionXEntry,
                            CamActionYEntry,
                            CamActionWEntry,
                            CamActionHEntry
                        )
                    )));

            int TextBoxWidthS = 120;
            AnimeActionIDEntry = new Entry();
            AnimeActionIDEntry.WidthRequest = TextBoxWidthS;
            AnimeActionXEntry = new Entry();
            AnimeActionXEntry.WidthRequest = TextBoxWidthS;
            AnimeActionYEntry = new Entry();
            AnimeActionYEntry.WidthRequest = TextBoxWidthS;
            AnimeActionSpeedXEntry = new Entry();
            AnimeActionSpeedXEntry.WidthRequest = TextBoxWidthS;
            AnimeActionSpeedYEntry = new Entry();
            AnimeActionSpeedYEntry.WidthRequest = TextBoxWidthS;
            AnimeActionAccelXEntry = new Entry();
            AnimeActionAccelXEntry.WidthRequest = TextBoxWidthS;
            AnimeActionAccelYEntry = new Entry();
            AnimeActionAccelYEntry.WidthRequest = TextBoxWidthS;
            AnimeActionLifetimeEntry = new Entry();
            AnimeActionLifetimeEntry.WidthRequest = TextBoxWidthS;
            AnimeActionStartFrameEntry = new Entry();
            AnimeActionStartFrameEntry.WidthRequest = TextBoxWidthS;

            AddAnimeActionFrame.Add(GuiUtils.CompactPackMultiV(false, false, 2, false, true, false, false, 4, 0,
                new Label("Create animation"),
                GuiUtils.CompactPackMultiH(false, false, 2, false, false, true, true, 0, 0,
                    GuiUtils.CompactPackMultiV(true, false, 2, false, false, false, false, 0, 0,
                            new Label("ID: "),
                            new Label("X: "),
                            new Label("Y: "),
                            new Label("X Speed: "),
                            new Label("Y Speed: "),
                            new Label("X Accel: "),
                            new Label("Y Accel: "),
                            new Label("Lifetime: "),
                            new Label("Starting frame: ")
                        ),
                    GuiUtils.CompactPackMultiV(true, false, 2, true, true, false, false, 0, 0,
                            AnimeActionIDEntry,
                            AnimeActionXEntry,
                            AnimeActionYEntry,
                            AnimeActionSpeedXEntry,
                            AnimeActionSpeedYEntry,
                            AnimeActionAccelXEntry,
                            AnimeActionAccelYEntry,
                            AnimeActionLifetimeEntry,
                            AnimeActionStartFrameEntry
                        )
                )));


            ActionsContainer.AddWithViewport(GuiUtils.CompactPackMultiV(false, false, 4, true, true, false, false, 2, 0, 
                AddTileActionFrame, 
                AddPathActionFrame,
                AddMusicActionFrame,
                AddPlayerActionFrame,
                AddCamActionFrame,
                AddAnimeActionFrame
                ));

            EventsContainer.PackStart(GuiUtils.CompactPackMultiV(false, false, 4, true, true, false, false, 4, 0,
                AddEventButton,
                RemoveEventButton,
                EventContainer,
                EventActionContainer,
                RemoveActionButton,
                ActionsContainer
                ));

            PathTriggerRadioButton = new RadioButton("Path");
            LevelTriggerRadioButton = new RadioButton(PathTriggerRadioButton, "Level");
            
            BindEventComboBox = new ComboBox(ProgramState.EventStore);
            CellRendererText __ComboBoxRenderer = new CellRendererText();
            BindEventComboBox.PackStart(__ComboBoxRenderer, false);
            BindEventComboBox.AddAttribute(__ComboBoxRenderer, "text", 0);
            BindEventComboBox.Active = 0;

            AddTriggerButton = new Button("Add Trigger");
            RemoveTriggerButton = new Button("Remove Trigger");

            TriggerViewContainer = new ScrolledWindow();
            TriggerView = new TreeView(ProgramState.PathTriggerStore);
            TriggerView.AppendColumn("Event", new CellRendererText(), "text", 0);
            TriggerView.HeadersVisible = false;
            TriggerView.HeightRequest = 128;
            TriggerViewContainer.Add(TriggerView);

            TriggerExpressions = new ListStore(typeof(string));
            TriggerExprViewContainer = new ScrolledWindow();
            TriggerExpressionView = new TreeView(TriggerExpressions);
            TriggerExpressionView.AppendColumn("Event", new CellRendererText(), "text", 0);
            TriggerExpressionView.HeadersVisible = false;
            TriggerExpressionView.HeightRequest = 128;
            TriggerExprViewContainer.Add(TriggerExpressionView);

            TriggerXPosRadioButton = new RadioButton("X");
            TriggerYPosRadioButton = new RadioButton(TriggerXPosRadioButton, "Y");

            TriggerEqRadioButton = new RadioButton("==");
            TriggerNeqRadioButton = new RadioButton(TriggerEqRadioButton, "~=");
            TriggerGrRadioButton = new RadioButton(TriggerEqRadioButton, ">");
            TriggerLsRadioButton = new RadioButton(TriggerEqRadioButton, "<");

            TriggerConditionCompareEntry = new Entry();

            AddTriggerCondCButton = new Button("Add Condition (Conjunctive)");
            AddTriggerCondDButton = new Button("Add Condition (Disjunctive)");
            ClearTriggerCondButton = new Button("Clear Conditions");

            TriggerExpressionContainer = new ScrolledWindow();
            TriggerExpressionContainer.HeightRequest = 198;
            AddTriggerExpressionButton = new Button("Add");
            RemoveTriggerExpressionButton = new Button("Remove");

            for (int i = 0; i < 9; i++)
                TriggerExpressionEntries[i] = new Entry();

            TriggerExpressionContainer.AddWithViewport(GuiUtils.CompactPackMultiV(false, false, 2, true, true, false, false, 2, 0,
                new Label("Custom Argument Expressions"),
                TriggerExpressionEntries[0],
                TriggerExpressionEntries[1],
                TriggerExpressionEntries[2],
                TriggerExpressionEntries[3],
                TriggerExpressionEntries[4],
                TriggerExpressionEntries[5],
                TriggerExpressionEntries[6],
                TriggerExpressionEntries[7],
                TriggerExpressionEntries[8],
                AddTriggerExpressionButton,
                RemoveTriggerExpressionButton
                ));

            TriggersContainer.PackStart(GuiUtils.CompactPackMultiV(false, false, 2, true, true, false, false, 4, 0,
                PathTriggerRadioButton,
                LevelTriggerRadioButton,
                GuiUtils.CompactPackMultiH(false, true, 0, true, true, true, true, 0, 0,
                    new Label("Bind Event: "),
                    BindEventComboBox
                    ),
                AddTriggerButton,
                RemoveTriggerButton,
                TriggerViewContainer,
                TriggerExprViewContainer,
                GuiUtils.CompactPackMultiH(true, true, 0, true, true, true, true, 0, 0,
                    TriggerXPosRadioButton,
                    TriggerYPosRadioButton
                ),
                GuiUtils.CompactPackMultiH(true, true, 0, true, true, true, true, 0, 0,
                    TriggerEqRadioButton,
                    TriggerNeqRadioButton
                ),
                GuiUtils.CompactPackMultiH(true, true, 0, true, true, true, true, 0, 0,
                    TriggerGrRadioButton,
                    TriggerLsRadioButton
                ),
                GuiUtils.CompactPackMultiH(false, true, 2, true, true, true, true, 0, 0,
                    new Label("To: "),
                    TriggerConditionCompareEntry
                ),
                AddTriggerCondCButton,
                AddTriggerCondDButton,
                ClearTriggerCondButton,
                TriggerExpressionContainer));

            MainSubContainer.PackStart(GuiUtils.CompactPack(ToolboxFrame, false, false, true, true, 4, 4));
            MainSubContainer.PackStart(GuiUtils.CompactPack(WorldScene, false, false, false, false, 8, 8));
            MainSubContainer.PackStart(GuiUtils.CompactPack(ActionNotebook, false, false, true, true, 4, 4));

            MainContainer.PackStart(MainMenuBar);
            MainContainer.PackStart(MainSubContainer);

            Add(MainContainer);

            WLDFilter = new FileFilter();
            WLDFilter.Name = "SMBX64 World Files (*.wld)";
            WLDFilter.AddPattern("*.wld");

            GLib.Timeout.Add(16, new TimeoutHandler(_OnTick));

            Grid4MenuItem.ButtonPressEvent += _OnGrid4;
            Grid8MenuItem.ButtonPressEvent += _OnGrid8;
            Grid16MenuItem.ButtonPressEvent += _OnGrid16;

            LoadMenuItem.ButtonPressEvent += _OnFileOpen;
            SaveMenuItem.ButtonPressEvent += _OnSave;
            ReloadMenuItem.ButtonPressEvent += _OnReload;
            WorldScene.ExposeEvent += _OnWorldDraw;
            AnimeToolbox.ExposeEvent += _OnAnimeToolboxDraw;
            CamToolbox.ExposeEvent += _OnCamToolboxDraw;
            PathToolbox.ExposeEvent += _OnPathToolboxDraw;
            CoinSignToolbox.ExposeEvent += _OnCoinSignToolboxDraw;
            WorldScene.ButtonPressEvent += _OnWorldSceneClick;
            WorldScene.ButtonReleaseEvent += _OnWorldSceneMouseRelease;
            WorldScene.MotionNotifyEvent += _OnWorldSceneMouseMove;
            SMBXWorldModeMenuItem.ButtonReleaseEvent += _OnModeToggle;
            XMapWorldModeMenuItem.ButtonReleaseEvent += _OnModeToggle;
            ItemConfigSaveButton.Released += _OnConfigSaveButtonClicked;
            AnimeToolbox.ButtonReleaseEvent += _OnAnimeToolboxClick;
            CamToolbox.ButtonReleaseEvent += _OnCamToolboxClick;
            PathToolbox.ButtonReleaseEvent += _OnPathToolboxClick;
            CoinSignToolbox.ButtonReleaseEvent += _OnCoinSignToolboxClick;

            WAnimeSpeedXEntry.TextInserted += _OnWanimeSpeedXChange;
            WAnimeSpeedXEntry.TextDeleted += _OnWanimeSpeedXChange;
            WAnimeSpeedYEntry.TextInserted += _OnWanimeSpeedYChange;
            WAnimeSpeedYEntry.TextDeleted += _OnWanimeSpeedYChange;
            WAnimeAccelXEntry.TextInserted += _OnWanimeAccelXChange;
            WAnimeAccelXEntry.TextDeleted += _OnWanimeAccelXChange;
            WAnimeAccelYEntry.TextInserted += _OnWanimeAccelYChange;
            WAnimeAccelYEntry.TextDeleted += _OnWanimeAccelYChange;
            WAnimeLifetimeEntry.TextInserted += _OnWanimeLifetimeChange;
            WAnimeLifetimeEntry.TextDeleted += _OnWanimeLifetimeChange;
            WAnimeStartFrameEntry.TextInserted += _OnWanimeStartFrameChange;
            WAnimeStartFrameEntry.TextDeleted += _OnWanimeStartFrameChange;

            CamWidthEntry.TextInserted += _OnCamWChange;
            CamWidthEntry.TextDeleted += _OnCamWChange;
            CamHeightEntry.TextInserted += _OnCamHChange;
            CamHeightEntry.TextDeleted += _OnCamHChange;

            SPathWaitEntry.TextInserted += _OnPathWaitChange;
            SPathWaitEntry.TextDeleted += _OnPathWaitChange;

            SignCoinsEntry.TextInserted += _OnSignCoinsChange;
            SignCoinsEntry.TextDeleted += _OnSignCoinsChange;

            AddEventButton.Released += _OnEventAdded;
            RemoveEventButton.Released += _OnEventRemoved;
            EventView.CursorChanged += _OnEventSelected;
            EventActionView.CursorChanged += _OnEventActionSelected;
            EventComboBox.Changed += _OnEventComboBoxChanged;

            AddTileActionButton.Pressed += _OnTileActionAdded;
            AddPathActionButton.Pressed += _OnPathActionAdded;
            AddMusicActionButton.Pressed += _OnMusicActionAdded;
            AddPlayerActionButton.Pressed += _OnPlayerActionAdded;
            AddCamActionButton.Pressed += _OnCamActionAdded;
            AddAnimeActionButton.Pressed += _OnAnimeActionAdded;

            RemoveActionButton.Pressed += _OnActionRemoved;

            PathTriggerRadioButton.Toggled += _OnPathTriggerSelected;
            LevelTriggerRadioButton.Toggled += _OnLevelTriggerSelected;
            AddTriggerButton.Pressed += _OnTriggerAdded;
            RemoveTriggerButton.Pressed += _OnTriggerRemoved;

            AddTriggerCondCButton.Pressed += _OnTriggerCondAddedC;
            AddTriggerCondDButton.Pressed += _OnTriggerCondAddedD;
            ClearTriggerCondButton.Pressed += _OnTriggerCondClear;
            TriggerView.CursorChanged += _OnTriggerSelected;

            AddTriggerExpressionButton.Pressed += _OnTriggerExprAdded;
            RemoveTriggerExpressionButton.Pressed += _OnTriggerExprRemoved;

            KeyPressEvent += _OnKeyPress;
            KeyReleaseEvent += _OnKeyRelease;
            Shown += _OnWindowShow;

            Focus = WorldScene;
        }

        [ConnectBefore]
        private void _OnClose(object sender, DeleteEventArgs e)
        {
            if (Drawing && UnsavedWorkPrompt() == ResponseType.No)
            {
                e.RetVal = true;
                return;
            }

            Application.Quit();
        }

        private void _OnReload(object sender, ButtonPressEventArgs e)
        {
            RefreshWorld();
        }

        private void _OnGrid4(object sender, ButtonPressEventArgs e)
        {
            GridX = 4;
            GridY = 4;
        }

        private void _OnGrid8(object sender, ButtonPressEventArgs e)
        {
            GridX = 8;
            GridY = 8;
        }

        private void _OnGrid16(object sender, ButtonPressEventArgs e)
        {
            GridX = 16;
            GridY = 16;
        }

        private ResponseType UnsavedWorkPrompt()
        {
            MessageDialog Warning = new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.YesNo, "Unsaved work will be lost. Proceed?");
            Warning.Title = "Warning";

            ResponseType R = (ResponseType)Warning.Run();
            Warning.Destroy();
            return R;
        }

        private void _OnFileOpen(object sender, ButtonPressEventArgs e)
        {
            if (Drawing && UnsavedWorkPrompt() == ResponseType.No)
                return;

            WorldState.SelectedWorldItem = null;
            GuiUtils.MultiHide(ItemPropertyMainLabel, ItemInfoLabel, EventLabel, EventComboBox, ItemConfigFrame, WAnimeInstConfigFrame);
            FileChooserDialog FileDialog = new FileChooserDialog("Open World File", this, FileChooserAction.Open, Stock.Cancel, ResponseType.Cancel, Stock.Open, ResponseType.Accept);
            FileDialog.AddFilter(WLDFilter);

            ResponseType Response = (ResponseType)FileDialog.Run();
            if (Response == ResponseType.Accept)
            {
                try
                {
                    string WorldFile = FileDialog.Filename;
                    FileDialog.Destroy();

                    WorldState.ReadWorld(WorldFile);

                    RefreshAnims();
                    Drawing = true;

                    ViewX = -560;
                    ViewY = -400;

                    ProgramState.LoadStockImages(@$".{ProgramState._SEP}res");
                    GuiUtils.SetSensitive(true, SaveMenuItem, ReloadMenuItem, ModeMenuItem, ToolboxFrame, ActionNotebook);
                }
                catch (Exception Ex)
                {
                    MessageDialog Error = new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, Ex.Message);
                    Error.Title = "Error";

                    Drawing = false;
                    GuiUtils.SetSensitive(false, SaveMenuItem, ReloadMenuItem, ModeMenuItem, ToolboxFrame, ActionNotebook);
                    Error.Run();
                    Error.Destroy();
                }
            }
            else
                FileDialog.Destroy();

            WorldScene.QueueDraw();
            AnimeToolbox.QueueDraw();
            PathToolbox.QueueDraw();
        }

        private void _OnSave(object sender, ButtonPressEventArgs e)
        {
            WorldState.SaveXMapData();
        }

        private void _OnWorldDraw(object sender, ExposeEventArgs e)
        {
            if (Drawing)
            {
                using (Cairo.Context GContext = Gdk.CairoHelper.Create(WorldScene.GdkWindow))
                {
                    GContext.SetSourceRGBA(0, 0, 0, 1);
                    GContext.Rectangle(160, 100, 800, 600);
                    GContext.Fill();

                    List<WorldItem> ItemsToDraw = new List<WorldItem>(
                        1 +
                        WorldState.LastUsefulTileIndex +
                        WorldState.LastUsefulSceneIndex +
                        WorldState.LastUsefulPathIndex +
                        WorldState.LastUsefulLevelIndex +
                        WorldState.LastUsefulMusicIndex +
                        WorldState.Animations.Count +
                        WorldState.CameraBoundaries.Count +
                        WorldState.SpecialPaths.Count +
                        WorldState.Signs.Count
                        );

                    for (int i = 0; i <= WorldState.LastUsefulTileIndex; i++)
                    {
                        Tile _Temp = WorldState.Tiles[i];
                        WorldItemConfig _CConfig = WorldState.TileConfig[_Temp.ID];

                        if (LogicUtils.RectIntersect(_Temp.X, _Temp.Y, _CConfig.Width, _CConfig.Height, ViewX, ViewY, ViewW, ViewH))
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                    }

                    for (int i = 0; i <= WorldState.LastUsefulSceneIndex; i++)
                    {
                        Scene _Temp = WorldState.Scenes[i];
                        WorldItemConfig _CConfig = WorldState.SceneConfig[_Temp.ID];

                        if (LogicUtils.RectIntersect(_Temp.X, _Temp.Y, _CConfig.Width, _CConfig.Height, ViewX, ViewY, ViewW, ViewH))
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                    }

                    for (int i = 0; i <= WorldState.LastUsefulPathIndex; i++)
                    {
                        WPath _Temp = WorldState.Paths[i];
                        WorldItemConfig _CConfig = WorldState.PathConfig[_Temp.ID];

                        if (LogicUtils.RectIntersect(_Temp.X, _Temp.Y, _CConfig.Width, _CConfig.Height, ViewX, ViewY, ViewW, ViewH))
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                    }

                    for (int i = 0; i <= WorldState.LastUsefulLevelIndex; i++)
                    {
                        Level _Temp = WorldState.Levels[i];
                        WorldItemConfig _CConfig = WorldState.LevelConfig[_Temp.ID];
                        WorldItemConfig _Config0 = WorldState.LevelConfig[0];
                        WorldItemConfig _Config29 = WorldState.LevelConfig[29];

                        Level _Attached0 = new Level(_Temp.Index, _Temp.X, _Temp.Y, 0, false, false);
                        Level _Attached29 = new Level(_Temp.Index, _Temp.X, (_Temp.Y + (_Config29.Height / 4)), 29, false, false);

                        int DrawX = _Temp.X;
                        int DrawY = _Temp.Y;
                        if (_CConfig.Width != 32) { DrawX -= (_CConfig.Width - 32) / 2; }
                        if (_CConfig.Height != 32) { DrawY -= _CConfig.Height - 32; }

                        int DrawW = _CConfig.Width;
                        int DrawH = _CConfig.Height;

                        if (_Temp.PathBackground)
                        {
                            if (_CConfig.Width < _Config0.Width)
                            {
                                DrawX -= _Config0.Width - _CConfig.Width;
                                DrawW = _Config0.Width;
                            }
                            if (_CConfig.Height < _Config0.Height)
                            {
                                DrawY -= _Config0.Height - _CConfig.Height;
                                DrawH = _Config0.Height;
                            }
                        }

                        if (_Temp.BigBackground)
                        {
                            if (DrawW < _Config29.Width)
                            {
                                DrawX -= _Config29.Width - DrawW;
                                DrawW = _Config29.Width;
                            }
                            if (DrawH < 3 * (_Config29.Height / 4))
                                DrawY -= (3 * (_Config29.Height / 4)) - DrawH;
                            DrawH += (_Config29.Height / 4);
                        }

                        if (LogicUtils.RectIntersect(DrawX, DrawY, DrawW, DrawH, ViewX, ViewY, ViewW, ViewH))
                        {
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                            if (_Temp.BigBackground)
                                LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Attached29);
                            if (_Temp.PathBackground)
                                LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Attached0);
                        }
                    }

                    for (int i = 0; i <= WorldState.LastUsefulMusicIndex; i++)
                    {
                        Music _Temp = WorldState.MusicBoxes[i];
                        WorldItemConfig _CConfig = WorldState.MusicConfig;

                        if (LogicUtils.RectIntersect(_Temp.X, _Temp.Y, _CConfig.Width, _CConfig.Height, ViewX, ViewY, ViewW, ViewH))
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                    }

                    for (int i = 0; i < WorldState.Animations.Count; i++)
                    {
                        Animation _Temp = WorldState.Animations[i];
                        WorldItemConfig _CConfig = WorldState.AnimeConfig[_Temp.ID];

                        if (LogicUtils.RectIntersect(_Temp.X, _Temp.Y, _CConfig.Width, _CConfig.Height, ViewX, ViewY, ViewW, ViewH))
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                    }

                    for (int i = 0; i < WorldState.CameraBoundaries.Count; i++)
                    {
                        CamBoundary _Temp = WorldState.CameraBoundaries[i];

                        if (LogicUtils.RectIntersect(_Temp.X, _Temp.Y, _Temp.Width, _Temp.Height, ViewX, ViewY, ViewW, ViewH))
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                    }

                    for (int i = 0; i < WorldState.SpecialPaths.Count; i++)
                    {
                        SPath _Temp = WorldState.SpecialPaths[i];

                        if (LogicUtils.RectIntersect(_Temp.X, _Temp.Y, 32, 32, ViewX, ViewY, ViewW, ViewH))
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                    }

                    for (int i = 0; i < WorldState.Signs.Count; i++)
                    {
                        CoinSign _Temp = WorldState.Signs[i];

                        if (LogicUtils.RectIntersect(_Temp.X, _Temp.Y, 32, 32, ViewX, ViewY, ViewW, ViewH))
                            LogicUtils.BinaryInsert<WorldItem>(ItemsToDraw, _Temp);
                    }

                    if (ItemsToDraw.Count > 0)
                    {
                        int MaxID = Math.Max(ItemsToDraw.Max(x => x.ID), (ushort)29);

                        Cairo.ImageSurface[] ClippedTileImages = new Cairo.ImageSurface[MaxID + 1];
                        Cairo.ImageSurface[] ClippedSceneImages = new Cairo.ImageSurface[MaxID + 1];
                        Cairo.ImageSurface[] ClippedPathImages = new Cairo.ImageSurface[MaxID + 1];
                        Cairo.ImageSurface[] ClippedLevelImages = new Cairo.ImageSurface[MaxID + 1];
                        Cairo.ImageSurface[] ClippedAnimeImages = new Cairo.ImageSurface[MaxID + 1];
                        Cairo.ImageSurface ClippedMusicImage = null;
                        Cairo.ImageSurface ClippedSelectionImage = null;

                        for (int i = 0; i < ItemsToDraw.Count; i++)
                        {
                            WorldItem DrawItem = ItemsToDraw[i];
                            Cairo.ImageSurface ItemImage;
                            Cairo.ImageSurface[] ClippedImages = null;
                            WorldItemConfig ItemConfig = DrawItem.ConfigArray()[DrawItem.ID];
                            int CurrentFrame = 0;

                            if (DrawItem.IsTile())
                            {
                                ItemImage = WorldState.TileImages[DrawItem.ID];
                                CurrentFrame = WorldState.TileFrameData[DrawItem.ID];
                                ClippedImages = ClippedTileImages;
                            }
                            else if (DrawItem.IsScene())
                            {
                                ItemImage = WorldState.SceneImages[DrawItem.ID];
                                CurrentFrame = WorldState.SceneFrameData[DrawItem.ID];
                                ClippedImages = ClippedSceneImages;
                            }
                            else if (DrawItem.IsPath())
                            {
                                ItemImage = WorldState.PathImages[DrawItem.ID];
                                CurrentFrame = WorldState.PathFrameData[DrawItem.ID];
                                ClippedImages = ClippedPathImages;
                            }
                            else if (DrawItem.IsLevel())
                            {
                                ItemImage = WorldState.LevelImages[DrawItem.ID];
                                CurrentFrame = WorldState.LevelFrameData[DrawItem.ID];
                                ClippedImages = ClippedLevelImages;
                            }
                            else if (DrawItem.IsMusic())
                            {
                                ItemImage = WorldState.MusicImage;
                                ItemConfig = WorldState.MusicConfig;
                                CurrentFrame = 0;
                            }
                            else if (DrawItem.IsAnime())
                            {
                                ItemImage = WorldState.AnimeImages[DrawItem.ID];
                                CurrentFrame = ((Animation)DrawItem).CurrentFrame;
                                ClippedImages = ClippedAnimeImages;
                            }
                            else
                            {
                                if (DrawItem.ID == 0)
                                    ItemImage = ProgramState.CameraImage;
                                else if (DrawItem.ID > 1)
                                    ItemImage = ProgramState.SPathImages[DrawItem.ID];
                                else
                                    ItemImage = ProgramState.SignImage;
                            }

                            if (DrawItem.IsMusic())
                            {
                                if (ClippedMusicImage == null)
                                {
                                    ClippedMusicImage = new Cairo.ImageSurface(Cairo.Format.ARGB32, ItemConfig.Width, ItemConfig.Height);
                                    Cairo.Surface PrevTarget = GContext.GetTarget();

                                    GContext.SetTarget(ClippedMusicImage);
                                    GContext.SetSourceRGBA(1, 0.12, 1, 0.2);
                                    GContext.Rectangle(0, 0, 32, 32);
                                    GContext.Fill();
                                    GContext.SetSourceSurface(ItemImage, 0, 0);
                                    GContext.Paint();
                                    GContext.SetSourceRGBA(1, 0.12, 1, 1);
                                    GContext.Rectangle(1, 1, 30, 30);
                                    GContext.Stroke();
                                    GContext.SetTarget(PrevTarget);
                                }

                                GContext.SetSourceSurface(ClippedMusicImage, DrawItem.X - ViewX, DrawItem.Y - ViewY);
                                GContext.Rectangle(0, 0, ViewW, ViewH);
                                GContext.Clip();
                                GContext.Paint();
                            }
                            else if (!DrawItem.IsMisc())
                            {
                                if (ClippedImages[DrawItem.ID] == null)
                                {
                                    ClippedImages[DrawItem.ID] = new Cairo.ImageSurface(Cairo.Format.ARGB32, ItemConfig.Width, ItemConfig.Height);
                                    Cairo.Surface PrevTarget = GContext.GetTarget();
                                    GContext.SetTarget(ClippedImages[DrawItem.ID]);

                                    if (ProgramState.SelectionStateToggle && DrawItem.IsAnime())
                                    {
                                        GContext.SetSourceRGBA(1, 1, 0.08, 0.2);
                                        GContext.Rectangle(0, 0, ItemConfig.Width, ItemConfig.Height);
                                        GContext.Fill();
                                    }

                                    GContext.SetSourceSurface(ItemImage, 0, -CurrentFrame * ItemConfig.Height);
                                    GContext.Paint();

                                    GContext.SetTarget(PrevTarget);
                                }

                                if (!DrawItem.IsLevel())
                                    GContext.SetSourceSurface(ClippedImages[DrawItem.ID], DrawItem.X - ViewX, DrawItem.Y - ViewY);
                                else
                                    GContext.SetSourceSurface(ClippedImages[DrawItem.ID], (DrawItem.X - ViewX) - (ItemConfig.Width - 32) / 2, (DrawItem.Y - ViewY) - (ItemConfig.Height - 32));
                                GContext.Rectangle(0, 0, ViewW, ViewH);
                                GContext.Clip();
                                GContext.Paint();
                            }
                            else
                            {
                                if (ProgramState.SelectionStateToggle)
                                {
                                    switch (DrawItem.ID)
                                    {
                                        case 0:
                                            GContext.SetSourceRGBA(0.08, 1, 1, 0.5);
                                            GContext.Rectangle(DrawItem.X - ViewX + 2, DrawItem.Y - ViewY + 2, 30, 30);
                                            GContext.Fill();
                                            break;
                                        case 1:
                                            GContext.SetSourceRGBA(1, 0.75, 0.25, 0.5);
                                            GContext.Rectangle(DrawItem.X - ViewX, DrawItem.Y - ViewY, 32, 32);
                                            GContext.Fill();
                                            break;
                                        case 2:
                                            GContext.SetSourceRGBA(1, 0, 0, 0.5);
                                            GContext.Rectangle(DrawItem.X - ViewX, DrawItem.Y - ViewY, 32, 32);
                                            GContext.Fill();
                                            break;
                                        case 3:
                                            GContext.SetSourceRGBA(0, 1, 0, 0.5);
                                            GContext.Rectangle(DrawItem.X - ViewX, DrawItem.Y - ViewY, 32, 32);
                                            GContext.Fill();
                                            break;
                                        case 4:
                                            GContext.SetSourceRGBA(0, 0, 1, 0.5);
                                            GContext.Rectangle(DrawItem.X - ViewX, DrawItem.Y - ViewY, 32, 32);
                                            GContext.Fill();
                                            break;
                                    }
                                }
                                GContext.SetSourceSurface(ItemImage, DrawItem.X - ViewX, DrawItem.Y - ViewY);
                                GContext.Rectangle(0, 0, ViewW, ViewH);
                                GContext.Clip();
                                GContext.Paint();
                                if (ProgramState.SelectionStateToggle && DrawItem.ID == 0)
                                {
                                    CamBoundary DC = (CamBoundary)DrawItem;
                                    GContext.SetSourceRGBA(0.08, 1, 1, 0.5);
                                    GContext.Rectangle(DC.X - ViewX + 1, DC.Y - ViewY + 1, DC.Width - 2, DC.Height - 2);
                                    GContext.Stroke();
                                }
                            }
                        }

                        if (WorldState.SelectedWorldItem != null)
                        {
                            WorldItem W = WorldState.SelectedWorldItem;
                            if (!W.IsMisc())
                            {
                                if (LogicUtils.RectIntersect(W.X, W.Y, W.ConfigArray()[W.ID].Width, W.ConfigArray()[W.ID].Height, ViewX, ViewY, ViewW, ViewH))
                                {
                                    if (!W.IsAnime())
                                    {
                                        ClippedSelectionImage = new Cairo.ImageSurface(Cairo.Format.ARGB32, 32, 32);
                                        Cairo.Surface PrevTarget = GContext.GetTarget();
                                        GContext.SetTarget(ClippedSelectionImage);
                                        GContext.SetSourceSurface(ProgramState.SelectedItemOverlay, 0, (int)(-((WorldState.CurrentTick % 12) / 2) * 32));
                                        GContext.Paint();
                                        GContext.SetTarget(PrevTarget);

                                        GContext.SetSourceSurface(ClippedSelectionImage, W.X - ViewX, W.Y - ViewY);
                                        GContext.Rectangle(0, 0, ViewW, ViewH);
                                        GContext.Clip();
                                        GContext.Paint();
                                    }
                                    else
                                    {
                                        GContext.SetSourceRGBA(1, 1, 0.08, 1);
                                        GContext.Rectangle(W.X - ViewX + 1, W.Y - ViewY + 1, W.ConfigArray()[W.ID].Width - 1, W.ConfigArray()[W.ID].Height - 1);
                                        GContext.Stroke();
                                    }
                                }
                            }
                            else
                            {
                                switch (W.ID)
                                {
                                    case 0:
                                        CamBoundary WC = (CamBoundary)W;
                                        if (LogicUtils.RectIntersect(WC.X, WC.Y, WC.Width, WC.Height, ViewX, ViewY, ViewW, ViewH))
                                        {
                                            GContext.SetSourceRGBA(0.08, 1, 1, 1);
                                            GContext.Rectangle(WC.X - ViewX + 1, WC.Y - ViewY + 1, WC.Width - 1, WC.Height - 1);
                                            GContext.Stroke();
                                        }
                                        break;
                                    case 1:
                                        if (LogicUtils.RectIntersect(W.X, W.Y, 32, 32, ViewX, ViewY, ViewW, ViewH))
                                        {
                                            GContext.SetSourceRGBA(1, 0.75, 0.25, 1);
                                            GContext.Rectangle(W.X - ViewX + 1, W.Y - ViewY + 1, 30, 30);
                                            GContext.Stroke();
                                        }
                                        break;
                                    case 2:
                                        if (LogicUtils.RectIntersect(W.X, W.Y, 32, 32, ViewX, ViewY, ViewW, ViewH))
                                        {
                                            GContext.SetSourceRGBA(1, 0, 0, 1);
                                            GContext.Rectangle(W.X - ViewX + 1, W.Y - ViewY + 1, 30, 30);
                                            GContext.Stroke();
                                        }
                                        break;
                                    case 3:
                                        if (LogicUtils.RectIntersect(W.X, W.Y, 32, 32, ViewX, ViewY, ViewW, ViewH))
                                        {
                                            GContext.SetSourceRGBA(0, 1, 0, 1);
                                            GContext.Rectangle(W.X - ViewX + 1, W.Y - ViewY + 1, 30, 30);
                                            GContext.Stroke();
                                        }
                                        break;
                                    case 4:
                                        if (LogicUtils.RectIntersect(W.X, W.Y, 32, 32, ViewX, ViewY, ViewW, ViewH))
                                        {
                                            GContext.SetSourceRGBA(0, 0, 1, 1);
                                            GContext.Rectangle(W.X - ViewX + 1, W.Y - ViewY + 1, 30, 30);
                                            GContext.Stroke();
                                        }
                                        break;
                                }
                            }
                        }

                        foreach (var S in ClippedTileImages) { if (S != null) { S.Dispose(); } }
                        foreach (var S in ClippedSceneImages) { if (S != null) { S.Dispose(); } }
                        foreach (var S in ClippedPathImages) { if (S != null) { S.Dispose(); } }
                        foreach (var S in ClippedLevelImages) { if (S != null) { S.Dispose(); } }
                        if (ClippedMusicImage != null) { ClippedMusicImage.Dispose(); }
                        if (ClippedSelectionImage != null) { ClippedSelectionImage.Dispose(); }
                        foreach (var S in ClippedAnimeImages) { if (S != null) { S.Dispose(); } }
                    }

                    if (Placing)
                    {
                        if (PlaceType == PLACE_TYPE_ANIME)
                        {
                            using (Cairo.ImageSurface ClippedAnimeImage = new Cairo.ImageSurface(Cairo.Format.ARGB32, WorldState.AnimeConfig[PlaceTypeID].Width, WorldState.AnimeConfig[PlaceTypeID].Height))
                            {
                                Cairo.Surface PrevTarget = GContext.GetTarget();
                                GContext.SetTarget(ClippedAnimeImage);
                                GContext.SetSourceSurface(WorldState.AnimeImages[PlaceTypeID], 0, 0);
                                GContext.Paint();

                                GContext.SetTarget(PrevTarget);

                                GContext.SetSourceSurface(ClippedAnimeImage, PlaceX - ViewX, PlaceY - ViewY);
                                GContext.Rectangle(0, 0, ViewW, ViewH);
                                GContext.Clip();
                                GContext.PaintWithAlpha(0.5);
                            }
                        }
                        else if (PlaceType == PLACE_TYPE_CAM)
                        {
                            using (Cairo.ImageSurface ClippedCamImage = new Cairo.ImageSurface(Cairo.Format.ARGB32, 32, 32))
                            {
                                Cairo.Surface PrevTarget = GContext.GetTarget();
                                GContext.SetTarget(ClippedCamImage);
                                GContext.SetSourceSurface(ProgramState.CameraImage, 0, 0);
                                GContext.Paint();

                                GContext.SetTarget(PrevTarget);

                                GContext.SetSourceSurface(ClippedCamImage, PlaceX - ViewX, PlaceY - ViewY);
                                GContext.Rectangle(0, 0, ViewW, ViewH);
                                GContext.Clip();
                                GContext.PaintWithAlpha(0.5);
                            }
                        }
                        else if (PlaceType == PLACE_TYPE_SPATH)
                        {
                            using (Cairo.ImageSurface ClippedPathImage = new Cairo.ImageSurface(Cairo.Format.ARGB32, 32, 32))
                            {
                                Cairo.Surface PrevTarget = GContext.GetTarget();
                                GContext.SetTarget(ClippedPathImage);
                                GContext.SetSourceSurface(ProgramState.SPathImages[PlaceTypeID], 0, 0);
                                GContext.Paint();

                                GContext.SetTarget(PrevTarget);

                                GContext.SetSourceSurface(ClippedPathImage, PlaceX - ViewX, PlaceY - ViewY);
                                GContext.Rectangle(0, 0, ViewW, ViewH);
                                GContext.Clip();
                                GContext.PaintWithAlpha(0.5);
                            }
                        }
                        else
                        {
                            using (Cairo.ImageSurface ClippedSignImage = new Cairo.ImageSurface(Cairo.Format.ARGB32, 32, 32))
                            {
                                Cairo.Surface PrevTarget = GContext.GetTarget();
                                GContext.SetTarget(ClippedSignImage);
                                GContext.SetSourceSurface(ProgramState.SignImage, 0, 0);
                                GContext.Paint();

                                GContext.SetTarget(PrevTarget);

                                GContext.SetSourceSurface(ClippedSignImage, PlaceX - ViewX, PlaceY - ViewY);
                                GContext.Rectangle(0, 0, ViewW, ViewH);
                                GContext.Clip();
                                GContext.PaintWithAlpha(0.5);
                            }
                        }
                    }

                    GContext.SetSourceRGBA(0.5, 0.5, 0.5, 0.7);
                    GContext.Rectangle(0, 0, 160, 700);
                    GContext.Fill();

                    GContext.SetSourceRGBA(0.5, 0.5, 0.5, 0.7);
                    GContext.Rectangle(160, 0, 960, 100);
                    GContext.Fill();

                    GContext.SetSourceRGBA(0.5, 0.5, 0.5, 0.7);
                    GContext.Rectangle(960, 100, 160, 700);
                    GContext.Fill();

                    GContext.SetSourceRGBA(0.5, 0.5, 0.5, 0.7);
                    GContext.Rectangle(0, 700, 960, 100);
                    GContext.Fill();

                    GContext.SetSourceRGBA(1, 0, 0, 1);
                    GContext.Rectangle(160, 100, 800, 600);
                    GContext.Stroke();
                }
            }
        }

        private bool _OnTick()
        {
            if (Drawing)
            {
                WorldState.Tick();

                for (int i = 1; i < WorldState.TileConfig.Length; i++)
                {
                    if (WorldState.CurrentTick % WorldState.TileConfig[i].FrameSpeed == 0)
                    {
                        WorldState.TileFrameData[i]++;
                        WorldState.TileFrameData[i] %= WorldState.TileConfig[i].Frames;
                    }
                }

                for (int i = 1; i < WorldState.SceneConfig.Length; i++)
                {
                    if (WorldState.CurrentTick % WorldState.SceneConfig[i].FrameSpeed == 0)
                    {
                        WorldState.SceneFrameData[i]++;
                        WorldState.SceneFrameData[i] %= WorldState.SceneConfig[i].Frames;
                    }
                }

                for (int i = 1; i < WorldState.PathConfig.Length; i++)
                {
                    if (WorldState.CurrentTick % WorldState.PathConfig[i].FrameSpeed == 0)
                    {
                        WorldState.PathFrameData[i]++;
                        WorldState.PathFrameData[i] %= WorldState.PathConfig[i].Frames;
                    }
                }

                for (int i = 0; i < WorldState.LevelConfig.Length; i++)
                {
                    if (WorldState.CurrentTick % WorldState.LevelConfig[i].FrameSpeed == 0)
                    {
                        WorldState.LevelFrameData[i]++;
                        WorldState.LevelFrameData[i] %= WorldState.LevelConfig[i].Frames;
                    }
                }

                for (int i = 0; i < WorldState.Animations.Count; i++)
                {
                    WorldState.Animations[i].Tick();
                }

                int XChg = 0, YChg = 0;
                if (!(KeyPressing(KEY_UP) && KeyPressing(KEY_DOWN)))
                {
                    if (KeyPressing(KEY_UP))
                    {
                        ViewY -= 16;
                        YChg = -16;
                    }
                    else if (KeyPressing(KEY_DOWN))
                    {
                        ViewY += 16;
                        YChg = 16;
                    }
                }

                if (!(KeyPressing(KEY_LEFT) && KeyPressing(KEY_RIGHT)))
                {
                    if (KeyPressing(KEY_LEFT))
                    {
                        ViewX -= 16;
                        XChg = -16;
                    }
                    else if (KeyPressing(KEY_RIGHT))
                    {
                        ViewX += 16;
                        XChg = 16;
                    }
                }

                if ((XChg != 0 || YChg != 0) && ProgramState.SelectionStateToggle && Dragging && WorldState.SelectedWorldItem != null)
                {
                    string Identifier = "";
                    if (WorldState.SelectedWorldItem.IsAnime())
                        Identifier = "Animation";
                    else
                    {
                        if (WorldState.SelectedWorldItem.ID == 0)
                            Identifier = "Camera Boundary";
                        else if (WorldState.SelectedWorldItem.ID == 1)
                            Identifier = "Star Coin Sign";
                        else
                            Identifier = "Special Path";
                    }
                    ((XMapWorldItem)WorldState.SelectedWorldItem).SetPos(WorldState.SelectedWorldItem.X + XChg, WorldState.SelectedWorldItem.Y + YChg);
                    ItemInfoLabel.Text = $"Type: {Identifier}\nID: {WorldState.SelectedWorldItem.ID}\nPosition: [{WorldState.SelectedWorldItem.X}, {WorldState.SelectedWorldItem.Y}]\nIndex: {WorldState.SelectedWorldItem.Index}";
                }

                ViewX = LogicUtils.Clamp(ViewX, -320000, 320000 - 32 - ViewW);
                ViewY = LogicUtils.Clamp(ViewY, -320000, 320000 - 32 - ViewH);

                if (Placing)
                {
                    PlaceX += XChg;
                    PlaceY += YChg;
                }

                WorldScene.QueueDraw();
            }
            return true;
        }

        [ConnectBefore]
        private void _OnKeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.Event.Key)
            {
                case Gdk.Key.Up:
                    Keys |= KEY_UP;
                    e.RetVal = true;
                    break;
                case Gdk.Key.Left:
                    Keys |= KEY_LEFT;
                    e.RetVal = true;
                    break;
                case Gdk.Key.Down:
                    Keys |= KEY_DOWN;
                    e.RetVal = true;
                    break;
                case Gdk.Key.Right:
                    Keys |= KEY_RIGHT;
                    e.RetVal = true;
                    break;
                default: break;
            }
        }

        [ConnectBefore]
        private void _OnKeyRelease(object sender, KeyReleaseEventArgs e)
        {
            switch (e.Event.Key)
            {
                case Gdk.Key.Up:
                    Keys &= ~KEY_UP;
                    e.RetVal = true;
                    break;
                case Gdk.Key.Left:
                    Keys &= ~KEY_LEFT;
                    e.RetVal = true;
                    break;
                case Gdk.Key.Down:
                    Keys &= ~KEY_DOWN;
                    e.RetVal = true;
                    break;
                case Gdk.Key.Right:
                    Keys &= ~KEY_RIGHT;
                    e.RetVal = true;
                    break;
                default: break;
            }
        }

        private void _OnModeToggle(object sender, ButtonReleaseEventArgs e)
        {
            CheckMenuItem ToggledItem = (CheckMenuItem)sender;

            if (ToggledItem == SMBXWorldModeMenuItem)
            {
                if (!SMBXWorldModeMenuItem.Active)
                    XMapWorldModeMenuItem.Active = false;
                else
                    SMBXWorldModeMenuItem.Active = false;

                WorldState.SelectedWorldItem = null;
                ProgramState.SetMode(false);
                GuiUtils.MultiHide(ItemPropertyMainLabel, ItemInfoLabel, EventLabel, EventComboBox, ItemConfigFrame, WAnimeInstConfigFrame);
                Dragging = false;
            }
            else
            {
                if (!XMapWorldModeMenuItem.Active)
                    SMBXWorldModeMenuItem.Active = false;
                else
                    XMapWorldModeMenuItem.Active = false;

                WorldState.SelectedWorldItem = null;
                GuiUtils.MultiHide(ItemPropertyMainLabel, ItemInfoLabel, EventLabel, EventComboBox, ItemConfigFrame, WAnimeInstConfigFrame);
                ProgramState.SetMode(true);
            }
        }

        private void SetItemConfigs()
        {
            ItemWidthEntry.Text = "";
            ItemHeightEntry.Text = "";
            ItemFramesEntry.Text = "";
            ItemFrameSpeedEntry.Text = "";
            ItemPriorityEntry.Text = "";
            ItemPFramesEntry.Text = "";

            WAnimeSpeedXEntry.Text = "";
            WAnimeSpeedYEntry.Text = "";
            WAnimeAccelXEntry.Text = "";
            WAnimeAccelYEntry.Text = "";
            WAnimeLifetimeEntry.Text = "";
            WAnimeStartFrameEntry.Text = "";

            CamWidthEntry.Text = "";
            CamHeightEntry.Text = "";

            SPathWaitEntry.Text = "";

            SignCoinsEntry.Text = "";

            if (WorldState.SelectedWorldItem != null)
            {
                if (!WorldState.SelectedWorldItem.IsMisc())
                {
                    WorldItemConfig Config = WorldState.SelectedWorldItem.ConfigArray()[WorldState.SelectedWorldItem.ID];
                    ItemWidthEntry.Text = Config.Width.ToString();
                    ItemHeightEntry.Text = Config.Height.ToString();
                    ItemFramesEntry.Text = Config.Frames.ToString();
                    ItemFrameSpeedEntry.Text = Config.FrameSpeed.ToString();
                    ItemPriorityEntry.Text = Config.Priority.ToString();
                    ItemPFramesEntry.Text = Config.PFrames.ToString();
                }
                if (ProgramState.SelectionStateToggle)
                {
                    if (WorldState.SelectedWorldItem.IsAnime())
                    {
                        Animation WA = (Animation)(WorldState.SelectedWorldItem);
                        WAnimeSpeedXEntry.Text = WA.SpeedX.ToString();
                        WAnimeSpeedYEntry.Text = WA.SpeedY.ToString();
                        WAnimeAccelXEntry.Text = WA.AccelX.ToString();
                        WAnimeAccelYEntry.Text = WA.AccelY.ToString();
                        WAnimeLifetimeEntry.Text = WA.Lifetime.ToString();
                        WAnimeStartFrameEntry.Text = WA.StartFrame.ToString();
                    }
                    else if (WorldState.SelectedWorldItem.ID == 0)
                    {
                        CamBoundary CB = (CamBoundary)(WorldState.SelectedWorldItem);
                        CamWidthEntry.Text = CB.Width.ToString();
                        CamHeightEntry.Text = CB.Height.ToString();
                    }
                    else if (WorldState.SelectedWorldItem.ID == 1)
                    {
                        CoinSign CS = (CoinSign)(WorldState.SelectedWorldItem);
                        SignCoinsEntry.Text = CS.ReqCoins.ToString();
                    }
                    else
                    {
                        SPath SP = (SPath)(WorldState.SelectedWorldItem);
                        SPathWaitEntry.Text = SP.WaitTimer.ToString();
                    }
                }
            }
        }

        private string FilterTextDbl(string S)
        {
            if (S.Length == 0) return S;

            string Minus = (S[0] == '-') ? "-" : "";
            string[] Parts = S.Split(".");
            StringBuilder SB = new StringBuilder();

            string Base = Parts[0];

            for (int i = 1; i < Parts.Length; i++)
                SB.Append(Parts[i]);

            string After = SB.ToString();
            string Dot = After.Length > 0 ? "." : "";

            return Minus + new string(Base.Where(x => Char.IsNumber(x)).ToArray()) + Dot + new string(After.Where(x => Char.IsNumber(x)).ToArray());
        }

        private string FilterTextInt(string S)
        {
            if (S.Length == 0) return S;

            string Minus = (S[0] == '-') ? "-" : "";
            S = new string(S.Where(x => Char.IsNumber(x)).ToArray());

            return Minus + S;
        }

        private string FilterTextUInt(string S)
        {
            return new string(S.Where(x => Char.IsNumber(x)).ToArray());
        }

        private void _OnWanimeSpeedXChange(object sender, EventArgs e)
        {
            if (WAnimeSpeedXEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsAnime())
            {
                string SpX = FilterTextDbl(WAnimeSpeedXEntry.Text);
                double D = double.NaN;
                double.TryParse(SpX, out D);
                if (D != double.NaN)
                    ((Animation)WorldState.SelectedWorldItem).SpeedX = D;
            }
        }

        private void _OnWanimeSpeedYChange(object sender, EventArgs e)
        {
            if (WAnimeSpeedYEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsAnime())
            {
                string SpY = FilterTextDbl(WAnimeSpeedYEntry.Text);
                double D = double.NaN;
                double.TryParse(SpY, out D);
                if (D != double.NaN)
                    ((Animation)WorldState.SelectedWorldItem).SpeedY = D;
            }
        }

        private void _OnWanimeAccelXChange(object sender, EventArgs e)
        {
            if (WAnimeAccelXEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsAnime())
            {
                string AcX = FilterTextDbl(WAnimeAccelXEntry.Text);
                double D = double.NaN;
                double.TryParse(AcX, out D);
                if (D != double.NaN)
                    ((Animation)WorldState.SelectedWorldItem).AccelX = D;
            }
        }

        private void _OnWanimeAccelYChange(object sender, EventArgs e)
        {
            if (WAnimeAccelYEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsAnime())
            {
                string AcY = FilterTextDbl(WAnimeAccelYEntry.Text);
                double D = double.NaN;
                double.TryParse(AcY, out D);
                if (D != double.NaN)
                    ((Animation)WorldState.SelectedWorldItem).AccelY = D;
            }
        }

        private void _OnWanimeLifetimeChange(object sender, EventArgs e)
        {
            if (WAnimeLifetimeEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsAnime())
            {
                string L = FilterTextUInt(WAnimeLifetimeEntry.Text);
                uint D = uint.MaxValue;
                uint.TryParse(L, out D);
                if (D != uint.MaxValue)
                    ((Animation)WorldState.SelectedWorldItem).Lifetime = D;
            }
        }

        private void _OnWanimeStartFrameChange(object sender, EventArgs e)
        {
            if (WAnimeStartFrameEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsAnime())
            {
                string SF = FilterTextUInt(WAnimeStartFrameEntry.Text);
                ushort D = ushort.MaxValue;
                ushort.TryParse(SF, out D);
                if (D != ushort.MaxValue)
                    ((Animation)WorldState.SelectedWorldItem).StartFrame = D;
            }
        }

        private void _OnCamWChange(object sender, EventArgs e)
        {
            if (CamWidthEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsMisc() && WorldState.SelectedWorldItem.ID == 0)
            {
                string W = FilterTextInt(CamWidthEntry.Text);
                int D = int.MinValue;
                int.TryParse(W, out D);
                if (D != int.MinValue)
                    ((CamBoundary)WorldState.SelectedWorldItem).Width = D;
            }
        }

        private void _OnCamHChange(object sender, EventArgs e)
        {
            if (CamHeightEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsMisc() && WorldState.SelectedWorldItem.ID == 0)
            {
                string H = FilterTextInt(CamHeightEntry.Text);
                int D = int.MinValue;
                int.TryParse(H, out D);
                if (D != int.MinValue)
                    ((CamBoundary)WorldState.SelectedWorldItem).Height = D;
            }
        }

        private void _OnPathWaitChange(object sender, EventArgs e)
        {
            if (SPathWaitEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsMisc() && WorldState.SelectedWorldItem.ID > 1)
            {
                string W = FilterTextInt(SPathWaitEntry.Text);
                int D = int.MinValue;
                int.TryParse(W, out D);
                if (D != int.MinValue)
                    ((SPath)WorldState.SelectedWorldItem).WaitTimer = D;
            }
        }

        private void _OnSignCoinsChange(object sender, EventArgs e)
        {
            if (SignCoinsEntry.Text != "" && WorldState.SelectedWorldItem != null && WorldState.SelectedWorldItem.IsMisc() && WorldState.SelectedWorldItem.ID == 1)
            {
                string H = FilterTextInt(SignCoinsEntry.Text);
                int D = int.MinValue;
                int.TryParse(H, out D);
                if (D != int.MinValue)
                    ((CoinSign)WorldState.SelectedWorldItem).ReqCoins = D;
            }
        }

        private void RefreshAnims()
        {
            int INC = 0;
            for (int i = 0; i < WorldState.AnimeImages.Length; i++)
            {
                if (WorldState.AnimeImages[i] != null)
                {
                    INC++;
                }
            }
            AnimeToolbox.HeightRequest = 32 * (INC / 7 + 1);
            CamToolbox.HeightRequest = 32;
            PathToolbox.HeightRequest = 32;
            CoinSignToolbox.HeightRequest = 32;
        }

        private void RefreshWorld()
        {
            if (UnsavedWorkPrompt() == ResponseType.No)
                return;

            WorldState.ReadWorld(System.IO.Path.Combine(ProgramState.CurrentEpisodeFolder, "world.wld"));
            RefreshAnims();

            WorldScene.QueueDraw();
            AnimeToolbox.QueueDraw();
            CamToolbox.QueueDraw();
            PathToolbox.QueueDraw();
            CoinSignToolbox.QueueDraw();
        }

        private void _OnWorldSceneClick(object sender, ButtonPressEventArgs e)
        {
            GuiUtils.MultiShow(ItemPropertyMainLabel, ItemInfoLabel, EventLabel, EventComboBox, ItemConfigFrame, WAnimeInstConfigFrame);
            GuiUtils.SetSensitive(true, EventLabel, EventComboBox, ItemPFramesEntry, ItemConfigFrame);
            GuiUtils.SetSensitive(false, WAnimeInstConfigFrame);

            Focus = WorldScene;

            if (!Placing)
            {
                if (Drawing && !ProgramState.SelectionStateToggle)
                {
                    for (int i = 0; i <= WorldState.LastUsefulLevelIndex; i++)
                    {
                        Level L = WorldState.Levels[i];
                        WorldItemConfig C = WorldState.LevelConfig[L.ID];
                        if (LogicUtils.RectIntersect((int)e.Event.X + ViewX, (int)e.Event.Y + ViewY, 1, 1, L.X, L.Y, 32, 32))
                        {
                            ItemInfoLabel.Text = $"Type: Level\nID: {L.ID}\nPosition: [{L.X}, {L.Y}]\nIndex: {L.Index}";

                            WorldState.SelectedWorldItem = L;
                            SetItemConfigs();
                            EventComboBox.Active = WorldState.LevelEventAssoc[L.Index];

                            return;
                        }
                    }

                    for (int i = 0; i <= WorldState.LastUsefulPathIndex; i++)
                    {
                        WPath P = WorldState.Paths[i];
                        WorldItemConfig C = WorldState.PathConfig[P.ID];
                        if (LogicUtils.RectIntersect((int)e.Event.X + ViewX, (int)e.Event.Y + ViewY, 1, 1, P.X, P.Y, C.Width, C.Height))
                        {
                            ItemInfoLabel.Text = $"Type: Path\nID: {P.ID}\nPosition: [{P.X}, {P.Y}]\nIndex: {P.Index}";

                            GuiUtils.SetSensitive(false, ItemPFramesEntry);
                            WorldState.SelectedWorldItem = P;
                            SetItemConfigs();
                            EventComboBox.Active = WorldState.PathEventAssoc[P.Index];

                            return;
                        }
                    }
                }
                else if (Drawing)
                {
                    GuiUtils.SetSensitive(true, WAnimeInstConfigFrame);
                    GuiUtils.SetSensitive(false, EventLabel, EventComboBox, ItemPFramesEntry);

                    for (int i = 0; i < WorldState.Animations.Count; i++)
                    {
                        Animation A = WorldState.Animations[i];
                        WorldItemConfig C = WorldState.AnimeConfig[A.ID];
                        if (LogicUtils.RectIntersect((int)e.Event.X + ViewX, (int)e.Event.Y + ViewY, 1, 1, A.X, A.Y, C.Width, C.Height))
                        {
                            if (e.Event.Button == 3)
                            {
                                int IDX = A.Index;
                                WorldState.Animations.Remove(A);
                                for (int j = IDX; j < WorldState.Animations.Count; j++)
                                    WorldState.Animations[j].CalibIndex();
                            }
                            else
                            {
                                Dragging = true;
                                ItemInfoLabel.Text = $"Type: Animation\nID: {A.ID}\nPosition: [{A.X}, {A.Y}]\nIndex: {A.Index}";

                                WorldState.SelectedWorldItem = A;
                                SetItemConfigs();
                                GuiUtils.SetSensitive(true, WAnimeSpeedXEntry, WAnimeSpeedYEntry, WAnimeAccelXEntry, WAnimeAccelYEntry, WAnimeLifetimeEntry, WAnimeStartFrameEntry);
                                GuiUtils.SetSensitive(false, CamWidthEntry, CamHeightEntry, SPathWaitEntry, SignCoinsEntry);

                                return;
                            }
                        }
                    }

                    for (int i = 0; i < WorldState.CameraBoundaries.Count; i++)
                    {
                        CamBoundary C = WorldState.CameraBoundaries[i];
                        if (LogicUtils.RectIntersect((int)e.Event.X + ViewX, (int)e.Event.Y + ViewY, 1, 1, C.X, C.Y, 32, 32))
                        {
                            if (e.Event.Button == 3)
                            {
                                int IDX = C.Index;
                                WorldState.CameraBoundaries.Remove(C);
                                for (int j = IDX; j < WorldState.CameraBoundaries.Count; j++)
                                    WorldState.CameraBoundaries[j].CalibIndex();
                            }
                            else
                            {
                                Dragging = true;
                                ItemInfoLabel.Text = $"Type: Camera Boundary\nID: 0\nPosition: [{C.X}, {C.Y}]\nIndex: {C.Index}";

                                WorldState.SelectedWorldItem = C;
                                SetItemConfigs();

                                GuiUtils.SetSensitive(false, ItemConfigFrame, WAnimeSpeedXEntry, WAnimeSpeedYEntry, WAnimeAccelXEntry, WAnimeAccelYEntry, WAnimeLifetimeEntry, WAnimeStartFrameEntry, SPathWaitEntry, SignCoinsEntry);
                                GuiUtils.SetSensitive(true, CamWidthEntry, CamHeightEntry);

                                return;
                            }
                        }
                    }

                    for (int i = 0; i < WorldState.SpecialPaths.Count; i++)
                    {
                        SPath SP = WorldState.SpecialPaths[i];
                        if (LogicUtils.RectIntersect((int)e.Event.X + ViewX, (int)e.Event.Y + ViewY, 1, 1, SP.X, SP.Y, 32, 32))
                        {
                            if (e.Event.Button == 3)
                            {
                                int IDX = SP.Index;
                                WorldState.SpecialPaths.Remove(SP);
                                for (int j = IDX; j < WorldState.SpecialPaths.Count; j++)
                                    WorldState.SpecialPaths[j].CalibIndex();
                            }
                            else
                            {
                                Dragging = true;
                                ItemInfoLabel.Text = $"Type: Special Path\nID: {SP.ID}\nPosition: [{SP.X}, {SP.Y}]\nIndex: {SP.Index}";

                                WorldState.SelectedWorldItem = SP;
                                SetItemConfigs();

                                GuiUtils.SetSensitive(false, ItemConfigFrame, WAnimeSpeedXEntry, WAnimeSpeedYEntry, WAnimeAccelXEntry, WAnimeAccelYEntry, WAnimeLifetimeEntry, WAnimeStartFrameEntry, CamWidthEntry, CamHeightEntry, SignCoinsEntry);
                                GuiUtils.SetSensitive(true, SPathWaitEntry);

                                return;
                            }
                        }
                    }

                    for (int i = 0; i < WorldState.Signs.Count; i++)
                    {
                        CoinSign CS = WorldState.Signs[i];
                        if (LogicUtils.RectIntersect((int)e.Event.X + ViewX, (int)e.Event.Y + ViewY, 1, 1, CS.X, CS.Y, 32, 32))
                        {
                            if (e.Event.Button == 3)
                            {
                                int IDX = CS.Index;
                                WorldState.Signs.Remove(CS);
                                for (int j = IDX; j < WorldState.Signs.Count; j++)
                                    WorldState.Signs[j].CalibIndex();
                            }
                            else
                            {
                                Dragging = true;
                                ItemInfoLabel.Text = $"Type: Star Coin Sign\nID: 1\nPosition: [{CS.X}, {CS.Y}]\nIndex: {CS.Index}";

                                WorldState.SelectedWorldItem = CS;
                                SetItemConfigs();

                                GuiUtils.SetSensitive(false, ItemConfigFrame, WAnimeSpeedXEntry, WAnimeSpeedYEntry, WAnimeAccelXEntry, WAnimeAccelYEntry, WAnimeLifetimeEntry, WAnimeStartFrameEntry, CamWidthEntry, CamHeightEntry, SPathWaitEntry);
                                GuiUtils.SetSensitive(true, SignCoinsEntry);

                                return;
                            }
                        }
                    }
                }
            }
            else
            {
                if (e.Event.Button != 3)
                {
                    if (PlaceType == PLACE_TYPE_ANIME)
                    {
                        WorldState.Animations.Add(new Animation(
                            WorldState.Animations.Count,
                            PlaceX,
                            PlaceY,
                            PlaceTypeID
                            ));
                    }
                    else if (PlaceType == PLACE_TYPE_CAM)
                    {
                        WorldState.CameraBoundaries.Add(new CamBoundary(
                            WorldState.CameraBoundaries.Count,
                            PlaceX,
                            PlaceY,
                            800,
                            600
                            ));
                    }
                    else if (PlaceType == PLACE_TYPE_SPATH)
                    {
                        WorldState.SpecialPaths.Add(new SPath(
                            WorldState.SpecialPaths.Count,
                            PlaceX,
                            PlaceY,
                            PlaceTypeID,
                            0
                            ));
                    }
                    else
                    {
                        WorldState.Signs.Add(new CoinSign(
                            WorldState.Signs.Count,
                            PlaceX,
                            PlaceY,
                            PlaceTypeID,
                            0
                            ));
                    }
                }
                Placing = false;

                AnimeToolbox.QueueDraw();
                CamToolbox.QueueDraw();
                PathToolbox.QueueDraw();
                CoinSignToolbox.QueueDraw();
            }
            WorldState.SelectedWorldItem = null;
            GuiUtils.MultiHide(ItemPropertyMainLabel, ItemInfoLabel, EventLabel, EventComboBox, ItemConfigFrame, WAnimeInstConfigFrame);
        }

        private void _OnWorldSceneMouseMove(object sender, MotionNotifyEventArgs e)
        {
            if (Dragging && WorldState.SelectedWorldItem != null && ProgramState.SelectionStateToggle)
            {
                int X = (int)e.Event.X / GridX * GridX + ViewX;
                int Y = (int)e.Event.Y / GridY * GridY + ViewY;

                if (DragOffsetX == 0 && DragOffsetY == 0)
                {
                    DragOffsetX = (X - WorldState.SelectedWorldItem.X) / GridX * GridX;
                    DragOffsetY = (Y - WorldState.SelectedWorldItem.Y) / GridY * GridY;
                }

                string Identifier = "";
                if (WorldState.SelectedWorldItem.IsAnime())
                    Identifier = "Animation";
                else
                {
                    if (WorldState.SelectedWorldItem.ID == 0)
                        Identifier = "Camera Boundary";
                    else if (WorldState.SelectedWorldItem.ID == 1)
                        Identifier = "Star Coin Sign";
                    else
                        Identifier = "Special Path";
                }
                ((XMapWorldItem)WorldState.SelectedWorldItem).SetPos(X - DragOffsetX, Y - DragOffsetY);
                ItemInfoLabel.Text = $"Type: {Identifier}\nID: {WorldState.SelectedWorldItem.ID}\nPosition: [{WorldState.SelectedWorldItem.X}, {WorldState.SelectedWorldItem.Y}]\nIndex: {WorldState.SelectedWorldItem.Index}";
            }

            if (Placing)
            {
                if (PlaceType == PLACE_TYPE_ANIME)
                {
                    PlaceX = (int)e.Event.X / GridX * GridX + ViewX - WorldState.AnimeConfig[PlaceTypeID].Width / 2;
                    PlaceY = (int)e.Event.Y / GridY * GridY + ViewY - WorldState.AnimeConfig[PlaceTypeID].Height / 2;
                }
                else
                {
                    PlaceX = (int)e.Event.X / GridX * GridX + ViewX - 16;
                    PlaceY = (int)e.Event.Y / GridY * GridY + ViewY - 16;
                }
            }
        }

        private void _OnWorldSceneMouseRelease(object sender, ButtonReleaseEventArgs e)
        {
            Dragging = false;
            DragOffsetX = 0;
            DragOffsetY = 0;
        }

        private void _OnAnimeToolboxClick(object sender, ButtonReleaseEventArgs e)
        {
            int X = (int)e.Event.X / 32;
            int Y = (int)e.Event.Y / 32;

            int CalcedAnimIndex = Y * 7 + (Math.Min(X, 6));
            int CID = -1;
            for (int i = 0; i < WorldState.AnimeImages.Length; i++)
            {
                if (WorldState.AnimeImages[i] != null)
                {
                    CID++;
                    if (CID == CalcedAnimIndex)
                    {
                        PlaceType = PLACE_TYPE_ANIME;
                        PlaceTypeID = (ushort)i;
                        Placing = true;
                    }
                }
            }

            CamToolbox.QueueDraw();
            AnimeToolbox.QueueDraw();
            PathToolbox.QueueDraw();
            CoinSignToolbox.QueueDraw();
        }

        private void _OnCamToolboxClick(object sender, ButtonReleaseEventArgs e)
        {
            int X = (int)e.Event.X / 32;
            int Y = (int)e.Event.Y / 32;

            if (X == 0 && Y == 0)
            {
                PlaceType = PLACE_TYPE_CAM;
                PlaceTypeID = 0;
                Placing = true;
            }

            CamToolbox.QueueDraw();
            AnimeToolbox.QueueDraw();
            PathToolbox.QueueDraw();
            CoinSignToolbox.QueueDraw();
        }

        private void _OnPathToolboxClick(object sender, ButtonReleaseEventArgs e)
        {
            int X = (int)e.Event.X / 32;
            int Y = (int)e.Event.Y / 32;

            if (X < 3)
            {
                PlaceType = PLACE_TYPE_SPATH;
                PlaceTypeID = (ushort)(X + 2);
                Placing = true;
            }

            CamToolbox.QueueDraw();
            AnimeToolbox.QueueDraw();
            PathToolbox.QueueDraw();
            CoinSignToolbox.QueueDraw();
        }

        private void _OnCoinSignToolboxClick(object sender, ButtonReleaseEventArgs e)
        {
            int X = (int)e.Event.X / 32;
            int Y = (int)e.Event.Y / 32;

            if (X == 0 && Y == 0)
            {
                PlaceType = PLACE_TYPE_SIGN;
                PlaceTypeID = 1;
                Placing = true;
            }

            CamToolbox.QueueDraw();
            AnimeToolbox.QueueDraw();
            PathToolbox.QueueDraw();
            CoinSignToolbox.QueueDraw();
        }

        private void _OnAnimeToolboxDraw(object sender, ExposeEventArgs e)
        {
            if (!Drawing)
                return;

            int INC = 0;
            using (Cairo.Context GContext = Gdk.CairoHelper.Create(AnimeToolbox.GdkWindow))
            {
                for (int i = 0; i < WorldState.AnimeImages.Length; i++)
                {
                    if (WorldState.AnimeImages[i] != null)
                    {
                        WorldItemConfig C = WorldState.AnimeConfig[i];
                        double ScaleX = 32.0 / C.Width;
                        double ScaleY = 32.0 / C.Height;
                        double Scale = Math.Min(ScaleX, ScaleY);
                        using (Cairo.ImageSurface ScaledImage = new Cairo.ImageSurface(Cairo.Format.ARGB32, (int)Math.Floor(C.Width * Scale), (int)Math.Floor(C.Height * Scale)))
                        {
                            Cairo.Surface PrevTarget = GContext.GetTarget();
                            GContext.SetTarget(ScaledImage);

                            GContext.Scale(Scale, Scale);
                            GContext.SetSourceSurface(WorldState.AnimeImages[i], 0, 0);
                            GContext.Paint();

                            GContext.SetTarget(PrevTarget);

                            if (Placing && PlaceType == PLACE_TYPE_ANIME && PlaceTypeID == i)
                            {
                                GContext.SetSourceRGBA(1, 1, 1, 0.5);
                                GContext.Rectangle((INC % 7) * 32, (INC / 7) * 32, 32, 32);
                                GContext.Fill();
                            }

                            GContext.SetSourceSurface(ScaledImage, (int)((INC % 7) * 32 + (32 - Scale * C.Width) / 2), (int)((INC / 7) * 32 + (32 - Scale * C.Height) / 2));
                            GContext.Paint();

                            if (Placing && PlaceType == PLACE_TYPE_ANIME && PlaceTypeID == i)
                            {
                                GContext.SetSourceRGBA(1, 1, 1, 1);
                                GContext.Rectangle((INC % 7) * 32 + 1, (INC / 7) * 32 + 1, 30, 30);
                                GContext.Stroke();
                            }
                        }
                        INC++;
                    }
                }
            }
        }

        private void _OnCamToolboxDraw(object sender, ExposeEventArgs e)
        {
            if (!Drawing)
                return;

            using (Cairo.Context GContext = Gdk.CairoHelper.Create(CamToolbox.GdkWindow))
            {
                if (Placing && PlaceType == PLACE_TYPE_CAM)
                {
                    GContext.SetSourceRGBA(1, 1, 1, 0.5);
                    GContext.Rectangle(0, 0, 32, 32);
                    GContext.Fill();
                }

                GContext.SetSourceSurface(ProgramState.CameraImage, 0, 0);
                GContext.Paint();

                if (Placing && PlaceType == PLACE_TYPE_CAM)
                {
                    GContext.SetSourceRGBA(1, 1, 1, 1);
                    GContext.Rectangle(1, 1, 30, 30);
                    GContext.Stroke();
                }
            }
        }

        private void _OnPathToolboxDraw(object sender, ExposeEventArgs e)
        {
            if (!Drawing)
                return;

            using (Cairo.Context GContext = Gdk.CairoHelper.Create(PathToolbox.GdkWindow))
            {
                for (int i = 2; i <= 4; i++)
                {
                    if (Placing && PlaceType == PLACE_TYPE_SPATH && PlaceTypeID == i)
                    {
                        GContext.SetSourceRGBA(1, 1, 1, 0.5);
                        GContext.Rectangle((i - 2) * 32, 0, 32, 32);
                        GContext.Fill();
                    }

                    GContext.SetSourceSurface(ProgramState.SPathImages[i], (i - 2) * 32, 0);
                    GContext.Paint();

                    if (Placing && PlaceType == PLACE_TYPE_SPATH && PlaceTypeID == i)
                    {
                        GContext.SetSourceRGBA(1, 1, 1, 1);
                        GContext.Rectangle((i - 2) * 32 + 1, 1, 30, 30);
                        GContext.Stroke();
                    }
                }
            }
        }

        private void _OnCoinSignToolboxDraw(object sender, ExposeEventArgs e)
        {
            if (!Drawing)
                return;

            using (Cairo.Context GContext = Gdk.CairoHelper.Create(CoinSignToolbox.GdkWindow))
            {
                if (Placing && PlaceType == PLACE_TYPE_SIGN)
                {
                    GContext.SetSourceRGBA(1, 1, 1, 0.5);
                    GContext.Rectangle(0, 0, 32, 32);
                    GContext.Fill();
                }

                GContext.SetSourceSurface(ProgramState.SignImage, 0, 0);
                GContext.Paint();

                if (Placing && PlaceType == PLACE_TYPE_SIGN)
                {
                    GContext.SetSourceRGBA(1, 1, 1, 1);
                    GContext.Rectangle(1, 1, 30, 30);
                    GContext.Stroke();
                }
            }
        }

        private void _OnWindowShow(object sender, EventArgs e)
        {
            GuiUtils.MultiHide(ItemPropertyMainLabel, ItemInfoLabel, EventLabel, EventComboBox, ItemConfigFrame, WAnimeInstConfigFrame);
        }

        private void _OnConfigSaveButtonClicked(object sender, EventArgs e)
        {
            WorldItem WItem = WorldState.SelectedWorldItem;
            if (WItem == null)
                return;

            ushort W = 0, H = 0, F = 0, FS = 0, PF = 0;
            double P = 0;

            bool GoodToGo =
                ushort.TryParse(ItemWidthEntry.Text, out W) &&
                ushort.TryParse(ItemHeightEntry.Text, out H) &&
                ushort.TryParse(ItemFramesEntry.Text, out F) &&
                ushort.TryParse(ItemFrameSpeedEntry.Text, out FS) &&
                double.TryParse(ItemPriorityEntry.Text, out P) &&
                ushort.TryParse(ItemPFramesEntry.Text, out PF);

            if (!ItemPFramesEntry.Sensitive)
                PF = 0;

            if (GoodToGo)
            {
                string SPath = "";

                if (WItem.IsLevel())
                    SPath = Level.LevelIdentifier(WItem.ID) + ".txt";
                else if (WItem.IsPath())
                    SPath = WPath.PathIdentifier(WItem.ID) + ".txt";

                string FPath = System.IO.Path.Combine(ProgramState.CurrentEpisodeFolder, SPath);

                WorldItemConfig.SaveConfiguration(FPath, W, H, F, FS, P, PF);
                RefreshWorld();
            }
            else
            {
                MessageDialog Error = new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Invalid format for one or more configuration parameters.");
                Error.Title = "Error";

                Error.Run();
                Error.Destroy();
            }
        }

        private void _OnEventAdded(object sender, EventArgs e)
        {
            ProgramState.AddEvent(new XEvent(ProgramState.Events.Count));
        }

        private void _OnEventRemoved(object sender, EventArgs e)
        {
            if (ProgramState.LevelTriggers.Any(x => x.TriggeredEvent == ProgramState.Events[CurrentEventIndex]) ||
                ProgramState.PathTriggers.Any(x => x.TriggeredEvent == ProgramState.Events[CurrentEventIndex]))
            {
                MessageDialog Error = new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Unable to remove this event because it has a trigger bound to it.");
                Error.Title = "Error";
                Error.Run();
                Error.Destroy();

                return;
            }


            if (EventComboBox.Active == CurrentEventIndex)
                EventComboBox.Active = 0;

            if (BindEventComboBox.Active == CurrentEventIndex)
                BindEventComboBox.Active = 0;

            ProgramState.RemoveEvent(CurrentEventIndex);

            for (int i = 0; i < WorldState.PathEventAssoc.Length; i++)
            {
                if (WorldState.PathEventAssoc[i] == CurrentEventIndex)
                    WorldState.PathEventAssoc[i] = 0;
                else if (WorldState.PathEventAssoc[i] > CurrentEventIndex)
                    WorldState.PathEventAssoc[i]--;
            }
            for (int i = 0; i < WorldState.LevelEventAssoc.Length; i++)
            {
                if (WorldState.LevelEventAssoc[i] == CurrentEventIndex)
                    WorldState.LevelEventAssoc[i] = 0;
                else if (WorldState.LevelEventAssoc[i] > CurrentEventIndex)
                    WorldState.LevelEventAssoc[i]--;
            }

            CurrentEventIndex = 0;
            GuiUtils.SetSensitive(false, AddTileActionFrame, AddPathActionFrame, AddMusicActionFrame, AddPlayerActionFrame, AddCamActionFrame, AddAnimeActionFrame);

            for (int i = 0; i < ProgramState.PathTriggers.Count; i++)
                GuiUtils.SetListVal(ProgramState.PathTriggerStore, i, ProgramState.PathTriggers[i].ToString());
            for (int i = 0; i < ProgramState.LevelTriggers.Count; i++)
                GuiUtils.SetListVal(ProgramState.LevelTriggerStore, i, ProgramState.LevelTriggers[i].ToString());

            EventActions.Clear();
        }

        private void _OnEventSelected(object sender, EventArgs e)
        {
            CurrentEventIndex = GuiUtils.TreeViewIndex(EventView);
            EventActions.Clear();
            if (CurrentEventIndex > 0)
            {
                foreach (XEventAction A in ProgramState.Events[CurrentEventIndex].Actions)
                    EventActions.AppendValues(A.ToLua());
                GuiUtils.SetSensitive(true, AddTileActionFrame, AddPathActionFrame, AddMusicActionFrame, AddPlayerActionFrame, AddCamActionFrame, AddAnimeActionFrame);
            }
            else
                GuiUtils.SetSensitive(false, AddTileActionFrame, AddPathActionFrame, AddMusicActionFrame, AddPlayerActionFrame, AddCamActionFrame, AddAnimeActionFrame);
        }

        private void _OnEventActionSelected(object sender, EventArgs e)
        {
            CurrentEventActionIndex = GuiUtils.TreeViewIndex(EventActionView);
        }

        private void _OnEventComboBoxChanged(object sender, EventArgs e)
        {
            if (WorldState.SelectedWorldItem != null)
            {
                if (WorldState.SelectedWorldItem.IsPath())
                    WorldState.PathEventAssoc[WorldState.SelectedWorldItem.Index] = EventComboBox.Active;
                else if(WorldState.SelectedWorldItem.IsLevel())
                    WorldState.LevelEventAssoc[WorldState.SelectedWorldItem.Index] = EventComboBox.Active;
            }
        }

        private void _OnTileActionAdded(object sender, EventArgs e)
        {
            if (CurrentEventIndex <= 0)
                return;

            int AType = 0;
            if (!TileActionTranslateRadioButton.Active)
            {
                AType++;
                if (!TileActionSetIDRadioButton.Active)
                    AType++;
            }

            bool OK = true;
            int LL = 6;
            if (AType == 1)
                LL--;

            int[] P = new int[6];

            for (int i = 0; i < LL; i++)
            {
                OK = OK && int.TryParse(FilterTextInt(TileActionParamEntries[i].Text), out P[i]);
            }

            if (OK)
            {
                ProgramState.Events[CurrentEventIndex].AddAction(new XTileAction(AType, !TileActionSaveCheckButton.Active, P));
                EventActions.AppendValues(ProgramState.Events[CurrentEventIndex].Actions[ProgramState.Events[CurrentEventIndex].Actions.Count - 1].ToLua());
            }
        }

        private void _OnPathActionAdded(object sender, EventArgs e)
        {
            if (CurrentEventIndex <= 0)
                return;

            int X = 0, Y = 0;
            bool OK = int.TryParse(FilterTextInt(PathActionXEntry.Text), out X) &&
                      int.TryParse(FilterTextInt(PathActionYEntry.Text), out Y);

            if (OK)
            {
                ProgramState.Events[CurrentEventIndex].AddAction(new XPathAction(X, Y));
                EventActions.AppendValues(ProgramState.Events[CurrentEventIndex].Actions[ProgramState.Events[CurrentEventIndex].Actions.Count - 1].ToLua());
            }
        }

        private void _OnMusicActionAdded(object sender, EventArgs e)
        {
            if (CurrentEventIndex <= 0)
                return;

            ProgramState.Events[CurrentEventIndex].AddAction(new XMusicAction((ushort)MusicActionSpinButton.ValueAsInt));
            EventActions.AppendValues(ProgramState.Events[CurrentEventIndex].Actions[ProgramState.Events[CurrentEventIndex].Actions.Count - 1].ToLua());
        }

        private void _OnPlayerActionAdded(object sender, EventArgs e)
        {
            if (CurrentEventIndex <= 0)
                return;

            ProgramState.Events[CurrentEventIndex].AddAction(new XPlayerAction(PlayerActionCheckBox.Active));
            EventActions.AppendValues(ProgramState.Events[CurrentEventIndex].Actions[ProgramState.Events[CurrentEventIndex].Actions.Count - 1].ToLua());
        }

        private void _OnCamActionAdded(object sender, EventArgs e)
        {
            if (CurrentEventIndex <= 0)
                return;

            int X = 0, Y = 0, W = 0, H = 0;

            bool OK = int.TryParse(FilterTextInt(CamActionXEntry.Text), out X) &&
                      int.TryParse(FilterTextInt(CamActionYEntry.Text), out Y) &&
                      int.TryParse(FilterTextInt(CamActionWEntry.Text), out W) &&
                      int.TryParse(FilterTextInt(CamActionHEntry.Text), out H);

            if (OK)
            {
                ProgramState.Events[CurrentEventIndex].AddAction(new XCamAction(X, Y, W, H));
                EventActions.AppendValues(ProgramState.Events[CurrentEventIndex].Actions[ProgramState.Events[CurrentEventIndex].Actions.Count - 1].ToLua());
            }
        }

        private void _OnAnimeActionAdded(object sender, EventArgs e)
        {
            if (CurrentEventIndex <= 0)
                return;

            uint Lifetime = 0;
            ushort StartFrame = 0;

            uint.TryParse(FilterTextUInt(AnimeActionLifetimeEntry.Text), out Lifetime);
            ushort.TryParse(FilterTextUInt(AnimeActionStartFrameEntry.Text), out StartFrame);

            ushort ID = 0;
            int X = 0, Y = 0;
            double SpdX = 0, SpdY = 0, AccX = 0, AccY = 0;

            bool OK = ushort.TryParse(FilterTextInt(AnimeActionIDEntry.Text), out ID) &&
                      int.TryParse(FilterTextInt(AnimeActionXEntry.Text), out X) &&
                      int.TryParse(FilterTextInt(AnimeActionYEntry.Text), out Y) &&
                      double.TryParse(FilterTextInt(AnimeActionSpeedXEntry.Text), out SpdX) &&
                      double.TryParse(FilterTextInt(AnimeActionSpeedYEntry.Text), out SpdY) &&
                      double.TryParse(FilterTextInt(AnimeActionAccelXEntry.Text), out AccX) &&
                      double.TryParse(FilterTextInt(AnimeActionAccelYEntry.Text), out AccY);

            if (OK)
            {
                ProgramState.Events[CurrentEventIndex].AddAction(new XAnimationAction(ID, X, Y, SpdX, SpdY, AccX, AccY, Lifetime, StartFrame));
                EventActions.AppendValues(ProgramState.Events[CurrentEventIndex].Actions[ProgramState.Events[CurrentEventIndex].Actions.Count - 1].ToLua());
            }
        }

        private void _OnActionRemoved(object sender, EventArgs e)
        {
            if (CurrentEventIndex <= 0 || CurrentEventActionIndex < 0)
                return;

            ProgramState.Events[CurrentEventIndex].RemoveAction(CurrentEventActionIndex);
            TreeIter _Iter;
            EventActions.IterNthChild(out _Iter, CurrentEventActionIndex);
            EventActions.Remove(ref _Iter);
            CurrentEventActionIndex = -1;
        }

        private void _OnPathTriggerSelected(object sender, EventArgs e)
        {
            if (PathTriggerRadioButton.Active)
            {
                TriggerView.Model = ProgramState.PathTriggerStore;
                TriggerView.GdkWindow.ProcessUpdates(true);
                TriggerExpressions.Clear();
            }
        }
        
        private void _OnLevelTriggerSelected(object sender, EventArgs e)
        {
            if (LevelTriggerRadioButton.Active)
            {
                TriggerView.Model = ProgramState.LevelTriggerStore;
                TriggerView.GdkWindow.ProcessUpdates(true);
                TriggerExpressions.Clear();
            }
        }

        private void _OnTriggerAdded(object sender, EventArgs e)
        {
            if (BindEventComboBox.Active > 0)
            {
                if(PathTriggerRadioButton.Active)
                {
                    ProgramState.AddPathTrigger(new XTrigger(ProgramState.Events[BindEventComboBox.Active]));
                }
                else
                {
                    ProgramState.AddLevelTrigger(new XTrigger(ProgramState.Events[BindEventComboBox.Active]));
                }
            }
        }

        private void _OnTriggerRemoved(object sender, EventArgs e)
        {
            int Sel = GuiUtils.TreeViewIndex(TriggerView);
            if (Sel >= 0)
            {
                if (PathTriggerRadioButton.Active)
                {
                    ProgramState.RemovePathTrigger(Sel);
                }
                else
                {
                    ProgramState.RemoveLevelTrigger(Sel);
                }
                TriggerExpressions.Clear();
            }
        }

        private void TAddTrigger(bool Conj)
        {
            int Sel = GuiUtils.TreeViewIndex(TriggerView);
            if (Sel >= 0)
            {
                int _Comp = 0;
                bool OK = int.TryParse(FilterTextInt(TriggerConditionCompareEntry.Text), out _Comp);

                if (!OK)
                    return;

                XTriggerConditionType XT = XTriggerConditionType.Equal;
                if (TriggerNeqRadioButton.Active)
                    XT = XTriggerConditionType.NotEqual;
                else if (TriggerGrRadioButton.Active)
                    XT = XTriggerConditionType.GreaterThan;
                else if (TriggerLsRadioButton.Active)
                    XT = XTriggerConditionType.LesserThan;

                if (PathTriggerRadioButton.Active)
                {
                    ProgramState.PathTriggers[Sel].AddCondition(XT, _Comp, Conj, TriggerXPosRadioButton.Active ? 0 : 1);
                    GuiUtils.SetListVal(ProgramState.PathTriggerStore, Sel, ProgramState.PathTriggers[Sel].ToString());
                }
                else
                {
                    ProgramState.LevelTriggers[Sel].AddCondition(XT, _Comp, Conj, TriggerXPosRadioButton.Active ? 0 : 1);
                    GuiUtils.SetListVal(ProgramState.LevelTriggerStore, Sel, ProgramState.LevelTriggers[Sel].ToString());
                }
            }
        }

        private void _OnTriggerCondAddedC(object sender, EventArgs e)
        {
            TAddTrigger(true);
        }

        private void _OnTriggerCondAddedD(object sender, EventArgs e)
        {
            TAddTrigger(false);
        }

        private void _OnTriggerCondClear(object sender, EventArgs e)
        {
            int Sel = GuiUtils.TreeViewIndex(TriggerView);
            if (Sel >= 0)
            {
                if (PathTriggerRadioButton.Active)
                {
                    ProgramState.PathTriggers[Sel].Conditions.Clear();
                    GuiUtils.SetListVal(ProgramState.PathTriggerStore, Sel, ProgramState.PathTriggers[Sel].ToString());
                }
                else
                {
                    ProgramState.LevelTriggers[Sel].Conditions.Clear();
                    GuiUtils.SetListVal(ProgramState.LevelTriggerStore, Sel, ProgramState.LevelTriggers[Sel].ToString());
                }
            }
        }

        private void _OnTriggerSelected(object sender, EventArgs e)
        {
            TriggerExpressions.Clear();
            int Sel = GuiUtils.TreeViewIndex(TriggerView);
            if (Sel >= 0)
            {
                if (PathTriggerRadioButton.Active)
                {
                    foreach (string[] F in ProgramState.PathTriggers[Sel].Arguments)
                        TriggerExpressions.AppendValues(String.Join(',', F));
                }
                else
                {
                    foreach (string[] F in ProgramState.LevelTriggers[Sel].Arguments)
                        TriggerExpressions.AppendValues(String.Join(',', F));
                }
            }
        }

        private void _OnTriggerExprAdded(object sender, EventArgs e)
        {
            int Sel = GuiUtils.TreeViewIndex(TriggerView);
            if (Sel >= 0)
            {
                if (PathTriggerRadioButton.Active)
                {
                    ProgramState.PathTriggers[Sel].Arguments.Add(new string[] {
                        TriggerExpressionEntries[0].Text,
                        TriggerExpressionEntries[1].Text,
                        TriggerExpressionEntries[2].Text,
                        TriggerExpressionEntries[3].Text,
                        TriggerExpressionEntries[4].Text,
                        TriggerExpressionEntries[5].Text,
                        TriggerExpressionEntries[6].Text,
                        TriggerExpressionEntries[7].Text,
                        TriggerExpressionEntries[8].Text
                    });
                    _OnTriggerSelected(TriggerView, EventArgs.Empty);
                }
                else
                {
                    ProgramState.LevelTriggers[Sel].Arguments.Add(new string[] {
                        TriggerExpressionEntries[0].Text,
                        TriggerExpressionEntries[1].Text,
                        TriggerExpressionEntries[2].Text,
                        TriggerExpressionEntries[3].Text,
                        TriggerExpressionEntries[4].Text,
                        TriggerExpressionEntries[5].Text,
                        TriggerExpressionEntries[6].Text,
                        TriggerExpressionEntries[7].Text,
                        TriggerExpressionEntries[8].Text
                    });
                    _OnTriggerSelected(TriggerView, EventArgs.Empty);
                }
            }
        }

        private void _OnTriggerExprRemoved(object sender, EventArgs e)
        {
            int Sel = GuiUtils.TreeViewIndex(TriggerView);
            int ASel = GuiUtils.TreeViewIndex(TriggerExpressionView);
            if (Sel >= 0 && ASel >= 0)
            {
                if (PathTriggerRadioButton.Active)
                {
                    ProgramState.PathTriggers[Sel].Arguments.RemoveAt(ASel);
                    _OnTriggerSelected(TriggerView, EventArgs.Empty);
                }
                else
                {
                    ProgramState.LevelTriggers[Sel].Arguments.RemoveAt(ASel);
                    _OnTriggerSelected(TriggerView, EventArgs.Empty);
                }
            }
        }
    }
}