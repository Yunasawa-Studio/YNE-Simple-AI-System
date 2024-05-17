﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;
using YNL.Editors.UIElements;
using YNL.Editors.Windows.Utilities;
using YNL.Extensions.Methods;

namespace YNL.SimpleAISystem.Editors
{
    public class WAIBehaviourEditor_Visual : VisualElement
    {
        #region ▶ Editor Contants
        private const string _windowIcon = "Textures/Editors/Scroll1";
        private const string _windowTitle = "AI Behaviour Editor";
        private const string _windowSubtitle = "Editor tool for creating or editing AI Behaviour";
        #endregion
        #region ▶ Visual Elements
        private WAIBehaviourEditor_Main _main;

        private EWindowTitle _windowTitlePanel;
        private EWindowTagPanel _tagPanel;

        private EInteractableImage _propertyPanel;
        private Image _behaviourPanel;
        public EAssetField<AIBehaviour> ReferencedBehaviour;
        public EStatePanel StatePanel;

        private VisualElement _handlerWindow;
        private Image _mainWindow;

        public EBoxPanel ActionPanel;
        public EBoxPanel TransitionPanel;
        #endregion
        #region ▶ Style Classes
        private const string _uss_styleSheet = "Style Sheets/WAIBehaviourEditor";

        private const string _uss_main = "Main";
        private const string _uss_propertyPanel = "PropertyPanel";
        private const string _uss_behaviourPanel = "BehaviourPanel";
        private const string _uss_referencedBehaviour = "BehaviourField";
        private const string _uss_windowTitlePanel = "WindowTitle";

        private const string _uss_handlerWindow = "HandlerWindow";
        private const string _uss_mainWindow = "MainWindow";
        #endregion
        #region ▶ Editor Properties
        private float _tagPanelWidth = 200;
        #endregion

        public WAIBehaviourEditor_Visual(EWindowTagPanel tagPanel, WAIBehaviourEditor_Main main) : base()
        {
            _tagPanel = tagPanel;
            _main = main;

            CreateElements();

            PanelMarginHandler();
            MainWindowHandler();

            this.AddElements(_handlerWindow, _windowTitlePanel, _propertyPanel);
        }

        private void CreateElements()
        {
            _windowTitlePanel = new EWindowTitle(_windowIcon.LoadResource<Texture2D>(), _windowTitle, _windowSubtitle).AddClass(_uss_windowTitlePanel);

            ReferencedBehaviour = new EAssetField<AIBehaviour>().AddClass(_uss_referencedBehaviour);
            ReferencedBehaviour.Background.OnDragPerform += _main.Handler.OnChangeBehaviour;

            _behaviourPanel = new Image().AddClass(_uss_behaviourPanel).AddElements(ReferencedBehaviour);

            StatePanel = new EStatePanel(new string[0], null);
            StatePanel.OnSelectState += _main.Handler.OnChangeState;

            _propertyPanel = new EInteractableImage().AddClass(_uss_propertyPanel).AddElements(_behaviourPanel, new ELine(ELineMode.Horizontal));
            _propertyPanel.AddElements(new Label("AI State").AddClass("StateTitle"), StatePanel);

            this.AddStyle(_uss_styleSheet, EAddress.USSFont).AddClass(_uss_main);
        }

        private void PanelMarginHandler()
        {
            _tagPanel.OnPointerEnter += () =>
            {
                _windowTitlePanel.Panel.SetMarginLeft(_tagPanelWidth - 150);
                _propertyPanel.SetMarginLeft(_tagPanelWidth).SetWidth(100);
                _mainWindow.SetMarginLeft(_tagPanelWidth + 100);
            };
            _tagPanel.OnPointerExit += () =>
            {
                _windowTitlePanel.Panel.SetMarginLeft(0);
                _propertyPanel.SetMarginLeft(50).SetWidth(200);
                _mainWindow.SetMarginLeft(_tagPanelWidth + 50);
            };
        }
        private void MainWindowHandler()
        {
            _handlerWindow = new VisualElement().AddClass(_uss_handlerWindow);
            _mainWindow = new Image().AddClass(_uss_mainWindow);

            ActionPanel = new EBoxPanel(true, _main).SetAlignSelf(Align.Center);
            TransitionPanel = new EBoxPanel(false, _main).SetAlignSelf(Align.FlexEnd);

            _mainWindow.AddElements(ActionPanel, TransitionPanel);

            _handlerWindow.AddElements(_mainWindow);
        }
        public void AddActionBoxes(params EActionBox[] boxes) => ActionPanel.AddBoxes(boxes);
        public void AddTransitionBoxes(params ETransitionBox[] boxes) => TransitionPanel.AddBoxes(boxes);
    }
}
#endif