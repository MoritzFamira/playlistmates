import React from 'react';
import SongItem from './SongItem';

function Playlist({ playlistData, onSongEdit, onSongDelete }) {
  return (
    <div className="playlist-card">
      <h2 className="playlist-title">{playlistData.title}</h2>
      <div className="songs-container">
        {playlistData.songs.map((song, index) => (
          <SongItem
            key={index}
            song={song}
            onEdit={() => onSongEdit(song)}
            onDelete={() => onSongDelete(song)}
          />
        ))}
      </div>
    </div>
  );
}

export default Playlist;
