using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vega.Controllers.Resources;
using Vega.Core;

namespace Vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to API Resource
            CreateMap<Make, MakeResource>();
            CreateMap<Make, KeyValuePairResource>();
            CreateMap<Model, KeyValuePairResource>();
            CreateMap<Feature, KeyValuePairResource>();
            CreateMap<Vehicle, VehicleResource>()
                    .ForMember(vehicleResource => vehicleResource.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                    .ForMember(vehicleResource => vehicleResource.Features, opt => opt.MapFrom(v => v.Features.Select(vehicleFeature => vehicleFeature.FeatureId)));
            CreateMap<Vehicle, SaveVehicleResource>()
                    .ForMember(vehicleResource => vehicleResource.Make, opt => opt.MapFrom(v=> v.Model.Make))
                    .ForMember(vehicleResource => vehicleResource.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
                    .ForMember(vehicleResource => vehicleResource.Features, opt => opt.MapFrom(v => v.Features.Select(vehicleFeature => new KeyValuePairResource {  Id = vehicleFeature.Feature.Id, Name = vehicleFeature.Feature.Name})));


            //API Resource to Domain
            CreateMap<VehicleResource, Vehicle>()
                    .ForMember(v => v.Id, opt => opt.Ignore())
                    .ForMember(v => v.ContactName, opt => opt.MapFrom(vehicleResource => vehicleResource.Contact.Name))
                    .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vehicleResource => vehicleResource.Contact.Email))
                    .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vehicleResource => vehicleResource.Contact.Phone))
                    //  .ForMember(v => v.Features, opt => opt.MapFrom(vehicleResource => vehicleResource.Features.Select(id => new VehicleFeature { FeatureId = id })));
                    // Code above removed to add that below. Avoiding duplicating the featureId (line 45)
                    .ForMember(v => v.Features, opt => opt.Ignore())
                    .AfterMap((vehicleResource, v) =>
                    {
                        //var removedFeatures = new List<VehicleFeature>();
                        //Remove unselected features
                        //foreach (var item in v.Features)
                        //    if (!vehicleResource.Features.Contains(item.FeatureId))                       
                        //        removedFeatures.Add(item);


                        // Using that below instead of the foreach above
                        var removedFeatures = v.Features.Where(f => !vehicleResource.Features.Contains(f.FeatureId)).ToList();

                        foreach (var item in removedFeatures)
                            v.Features.Remove(item);

                        //Add new features
                        //foreach (var id in vehicleResource.Features)
                        //    if (!v.Features.Any(f => f.FeatureId == id))
                        //        v.Features.Add(new VehicleFeature { FeatureId = id });

                        // Using that below instead of the foreach above
                        var addedFeatures = vehicleResource.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).ToList();
                        foreach (var id in addedFeatures)
                                v.Features.Add(new VehicleFeature { FeatureId = id });

                    });
        }
    }
}
