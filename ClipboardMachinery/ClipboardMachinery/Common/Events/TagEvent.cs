﻿using ClipboardMachinery.Components.Tag;
using ClipboardMachinery.Components.TagType;

namespace ClipboardMachinery.Common.Events {

    public class TagEvent {

        public enum TagEventType {
            TagAdded,
            TagRemoved,
            TagValueChanged,
            TypeAdded,
            TypeRemoved,
            TypeColorChanged,
            TypeDescriptionChanged,
            TypePriorityChanged
        }

        #region Properties

        public TagEventType EventType { get; }

        public int? TagId { get; private set; }

        public string TagName { get; private set; }

        public int? RelatedClipId { get; private set; }

        public object Argument { get; private set; }

        #endregion

        private TagEvent(TagEventType eventType) {
            EventType = eventType;
        }

        #region Factories

        public static TagEvent CreateTagAddedEvent(int clipId, TagModel tag) {
            return new TagEvent(TagEventType.TagAdded) {
                TagId = tag.Id,
                TagName = tag.Name,
                RelatedClipId = clipId,
                Argument = tag
            };
        }

        public static TagEvent CreateTagRemovedEvent(TagModel tag) {
            return new TagEvent(TagEventType.TagRemoved) {
                TagId = tag.Id,
                TagName = tag.Name
            };
        }

        public static TagEvent CreateTagValueChangedEvent(TagModel tag) {
            return new TagEvent(TagEventType.TagValueChanged) {
                TagId = tag.Id,
                TagName = tag.Name,
                Argument = tag.Value
            };
        }

        public static TagEvent CreateTypeAddedEvent(TagTypeModel tagType) {
            return new TagEvent(TagEventType.TypeAdded) {
                TagName = tagType.Name,
                Argument = tagType
            };
        }

        public static TagEvent CreateTypeDescriptionChangedEvent(TagTypeModel tagType) {
            return new TagEvent(TagEventType.TypeDescriptionChanged) {
                TagName = tagType.Name,
                Argument = tagType.Description
            };
        }

        public static TagEvent CreateTypePriorityChangedEvent(TagTypeModel tagType) {
            return new TagEvent(TagEventType.TypePriorityChanged) {
                TagName = tagType.Name,
                Argument = tagType.Priority
            };
        }

        public static TagEvent CreateTypeColorChangedEvent(TagTypeModel tagType) {
            return new TagEvent(TagEventType.TypeColorChanged) {
                TagName = tagType.Name,
                Argument = tagType.Color
            };
        }

        public static TagEvent CreateTypeRemovedEvent(TagTypeModel tagType) {
            return new TagEvent(TagEventType.TypeRemoved) {
                TagName = tagType.Name,
                Argument = tagType
            };
        }

        #endregion

    }

}
