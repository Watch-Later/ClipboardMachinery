﻿using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ClipboardMachinery.Components.Clip;
using ClipboardMachinery.Components.Tag;
using ClipboardMachinery.Components.TagType;
using ClipboardMachinery.Core.DataStorage;
using ClipboardMachinery.Core.DataStorage.Schema;
using ClipboardMachinery.Core.TagKind;
using ClipboardMachinery.Plumbing.Customization;

namespace ClipboardMachinery.Plumbing.Installers {

    [InstallerPriority(200)]
    public class MappingInstaller : IWindsorInstaller {

        public void Install(IWindsorContainer container, IConfigurationStore store) {
            container.Register(Component.For<IMapper>().Instance(
                new MapperConfiguration(config => ConfigureMapper(config, container.Resolve<ITagKindManager>())).CreateMapper()
            ));
        }

        private static void ConfigureMapper(IMapperConfigurationExpression config, ITagKindManager tagKindManager) {
                // Mappings from database to view
                config
                    .CreateMap<Clip, ClipModel>();

                config
                    .CreateMap<Color, System.Windows.Media.Color>()
                    .ForMember(c => c.ScA, opt => opt.Ignore())
                    .ForMember(c => c.ScR, opt => opt.Ignore())
                    .ForMember(c => c.ScG, opt => opt.Ignore())
                    .ForMember(c => c.ScB, opt => opt.Ignore());

                config
                    .CreateMap<Tag, TagModel>()
                    .ForMember(dest => dest.Value, opt => opt.MapFrom(source => tagKindManager.Read(source.Type.Name, source.Type.Kind, source.Value)))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Type.Name))
                    .ForMember(dest => dest.Kind, opt => opt.MapFrom(source => source.Type.Kind))
                    .ForMember(dest => dest.Priority, opt => opt.MapFrom(source => source.Type.Priority))
                    .ForMember(dest => dest.Description, opt => {
                        opt.MapFrom(source => source.Type.Description);
                        opt.AllowNull();
                    })
                    .ForMember(dest => dest.Color, opt => {
                        opt.MapFrom(source => source.Type.Color);
                        opt.AllowNull();
                    });

                config
                    .CreateMap<TagType, TagTypeModel>();

                // Mappings from view to database
                config
                    .CreateMap<System.Windows.Media.Color, Color>();

                /*
                config
                    .CreateMap<ClipModel, Clip>()
                    .AfterMap((source, desct) => {
                        foreach (Tag tag in desct.Tags) {
                            tag.ClipId = desct.Id;
                        }
                    });

                config
                    .CreateMap<TagModel, Tag>()
                    .ForMember(dest => dest.TagTypeName, opt => opt.MapFrom(source => source.TagTypeName))
                    .ForPath(dest => dest.Type.TagTypeName, opt => opt.MapFrom(source => source.TagTypeName))
                    .ForPath(dest => dest.Type.Color, opt => opt.MapFrom(source => source.Color))
                    .ForPath(dest => dest.Type.Type, opt => opt.MapFrom(source => source.Value.GetType()));
                */
        }

    }
}
