using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor.Installer;

namespace ClipboardMachinery.Plumbing.Customization {

    public class WindsorBootstrap : InstallerFactory {

        public override IEnumerable<Type> Select(IEnumerable<Type> installerTypes) {
            return installerTypes.OrderBy(GetPriority);
        }

        private int GetPriority(Type type) {
            return !(type.GetCustomAttributes(typeof(InstallerPriorityAttribute), false).FirstOrDefault() is InstallerPriorityAttribute attribute)
                ? InstallerPriorityAttribute.DEFAULT_PRIORITY
                : attribute.Priority;
        }

    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class InstallerPriorityAttribute : Attribute {

        public const int DEFAULT_PRIORITY = 100;

        public int Priority { get; }

        public InstallerPriorityAttribute(int priority) {
            Priority = priority;
        }

    }

}
