using System;
using Caliburn.Micro;
using System.Windows.Media;
using ClipboardMachinery.Core;

namespace ClipboardMachinery.Components.Tag {

    public class TagModel : PropertyChangedBase {

        #region Properties

        public int Id {
            get => id;
            set {
                if (id == value) {
                    return;
                }

                id = value;
                NotifyOfPropertyChange();
            }
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

        public object Value {
            get => val;
            set {
                if (val == value) {
                    return;
                }

                val = value;
                NotifyOfPropertyChange();
            }
        }

        public Type Kind {
            get => kind;
            set {
                if (kind == value) {
                    return;
                }

                kind = value;
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

        public byte Priority {
            get => priority;
            set {
                if (priority == value) {
                    return;
                }

                priority = value;
                NotifyOfPropertyChange();
            }
        }

        public Color? Color {
            get => color ?? defaultColor;
            set {
                if (color == value) {
                    return;
                }

                color = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Fields

        private static readonly Color defaultColor = System.Windows.Media.Color.FromArgb(
            SystemTagTypes.DefaultColor.A,
            SystemTagTypes.DefaultColor.R,
            SystemTagTypes.DefaultColor.G,
            SystemTagTypes.DefaultColor.B
        );

        private int id;
        private string name;
        private object val;
        private Type kind;
        private Color? color;
        private string description;
        private byte priority;

        #endregion

        public TagModel() {
        }

    }

}
