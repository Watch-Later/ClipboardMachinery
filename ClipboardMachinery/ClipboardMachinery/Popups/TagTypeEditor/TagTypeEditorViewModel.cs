﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using ClipboardMachinery.Common.Events;
using ClipboardMachinery.Components.Buttons.ActionButton;
using ClipboardMachinery.Components.ColorGallery;
using ClipboardMachinery.Components.Popup;
using ClipboardMachinery.Components.TagKind;
using ClipboardMachinery.Components.TagType;
using ClipboardMachinery.Core.DataStorage;
using static ClipboardMachinery.Common.Events.TagEvent;

namespace ClipboardMachinery.Popups.TagTypeEditor {

    public class TagTypeEditorViewModel : Screen, IPopupExtendedControls {

        #region Properties

        public BindableCollection<ActionButtonViewModel> PopupControls {
            get;
        }

        public TagTypeModel Model {
            get;
        }

        public string Name {
            get => name;
            set {
                if (name == value) {
                    return;
                }

                name = value;
                NotifyOfPropertyChange();
            }
        }

        public string Description {
            get => description;
            set {
                if (description == value) {
                    return;
                }

                description = value;
                NotifyOfPropertyChange();
            }
        }

        public ITagKindSchema SelectedTagKind {
            get => selectedTagKind;
            set {
                if (selectedTagKind == value) {
                    return;
                }

                selectedTagKind = value;
                NotifyOfPropertyChange();
            }
        }

        public ITagKindHandler TagKindHandler {
            get;
        }

        public ColorGalleryViewModel ColorGallery {
            get;
        }

        public bool IsSystemOwned {
            get;
        }

        public bool IsCreatingNew {
            get;
        }

        #endregion

        #region Fields

        private readonly IEventAggregator eventAggregator;
        private readonly IDataRepository dataRepository;

        private string name;
        private string description;
        private ITagKindSchema selectedTagKind;

        #endregion

        public TagTypeEditorViewModel(
            TagTypeModel tagTypeModel, bool isCreatingNew, ColorGalleryViewModel colorGallery, IEventAggregator eventAggregator, IDataRepository dataRepository,
            ITagKindHandler tagKindHandler, Func<ActionButtonViewModel> actionButtonFactory) {

            Model = tagTypeModel;
            Name = Model.Name;
            Description = Model.Description;
            TagKindHandler = tagKindHandler;
            IsSystemOwned = SystemTagTypes.TagTypes.Any(tt => tt.Name == Model.Name);
            PopupControls = new BindableCollection<ActionButtonViewModel>();
            this.eventAggregator = eventAggregator;
            this.dataRepository = dataRepository;
            IsCreatingNew = isCreatingNew;

            // Setup color gallery
            ColorGallery = colorGallery;
            ColorGallery.SelectColor(isCreatingNew ? SystemTagTypes.DefaultColor : Model.Color);

            // Create popup controls
            // Do not create remove button if edited tag belongs to the system since system owned tags can't be removed.
            if (!IsSystemOwned && !isCreatingNew) {
                ActionButtonViewModel removeButton = actionButtonFactory.Invoke();
                removeButton.ToolTip = "Remove";
                removeButton.Icon = (Geometry) Application.Current.FindResource("IconRemove");
                removeButton.HoverColor = (SolidColorBrush) Application.Current.FindResource("DangerousActionBrush");
                removeButton.ClickAction = OnRemoveClick;
                removeButton.ConductWith(this);
                PopupControls.Add(removeButton);
            }

            ActionButtonViewModel saveButton = actionButtonFactory.Invoke();
            saveButton.ToolTip = "Save";
            saveButton.Icon = (Geometry) Application.Current.FindResource("IconSave");
            saveButton.HoverColor = (SolidColorBrush) Application.Current.FindResource("ElementSelectBrush");
            saveButton.ClickAction = OnSaveClick;
            saveButton.ConductWith(this);
            PopupControls.Add(saveButton);
        }

        #region Handlers

        protected override Task OnActivateAsync(CancellationToken cancellationToken) {
            SelectedTagKind = TagKindHandler.FromType(Model.Kind);
            return base.OnActivateAsync(cancellationToken);
        }

        private Task OnRemoveClick(ActionButtonViewModel button) {
            return Task.CompletedTask;
        }

        private async Task OnSaveClick(ActionButtonViewModel button) {
            if (IsCreatingNew) {
                Model.Name = name;
                Model.Description = Description;
                Model.Kind = SelectedTagKind.Type;
                Model.Color = ColorGallery.SelectedColor;
            } else {
                // Update description if changed
                if (Model.Description != Description) {
                    Model.Description = Description;
                    await dataRepository.UpdateTagType(Model.Name, Description);
                    await eventAggregator.PublishOnCurrentThreadAsync(new TagEvent(TagEventType.DescriptionChange, Model.Name, Description));
                }

                // Update color if changed
                if (Model.Color != ColorGallery.SelectedColor) {
                    Model.Color = ColorGallery.SelectedColor;
                    await dataRepository.UpdateTagType(Model.Name, Model.Color);
                    await eventAggregator.PublishOnCurrentThreadAsync(new TagEvent(TagEventType.ColorChange, Model.Name, ColorGallery.SelectedColor));
                }
            }


            await eventAggregator.PublishOnCurrentThreadAsync(PopupEvent.Close());
        }

        #endregion

    }

}
