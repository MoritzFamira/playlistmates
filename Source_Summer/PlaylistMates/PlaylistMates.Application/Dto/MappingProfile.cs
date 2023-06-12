using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlaylistMates.Application.Model;

namespace PlaylistMates.Application.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SongDto, Song>();
            CreateMap<Song, SongDto>();
            CreateMap<Playlist, PlaylistDto>();
            CreateMap<PlaylistDto, Playlist>();
        }
    }

}
