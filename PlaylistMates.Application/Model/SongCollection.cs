﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistMates.Application.Model
{
    public class SongCollection : IEntity<int>
    {
        public SongCollection(int id, string title, DateTime creationDate)
        {
            Id = id;
            Title = title;
            CreationDate = creationDate;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected SongCollection() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        
        protected List<Song> _songs = new();
        public virtual IReadOnlyCollection<Song> Songs => _songs;

        public void AddSong(Song song)
        {
            _songs.Add(song);
        }
    }
}