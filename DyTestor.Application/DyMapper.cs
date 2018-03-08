using AutoMapper;
using AutoMapper.Configuration;
using DyTestor.DataObject;
using DyTestor.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DyTestor.Application
{
    public static class DyMapper
    {
        static DyMapper()
        {
            MapperConfigurationExpression cfg = new MapperConfigurationExpression();

            cfg.CreateMap<QRCodeDataObject, QRCode>();
            cfg.CreateMap<QRCode, QRCodeDataObject>();
            
            Mapper.Initialize(cfg);
        }

        public static Target Map<Source,Target>(Source source)
        {
            return source == null ? default(Target) : Mapper.Map<Source, Target>(source);
        }

        public static Target Map<Source,Target>(Source source,Target target)
        {
            return source == null ? target : Mapper.Map(source, target);
        }
    }
}
