import React, { useState } from 'react';
import SongItem from './SongItem';

function Playlist({ playlistData, onSongEdit, onSongDelete, onEdit, onDelete }) {
  const [isExpanded, setIsExpanded] = useState(false);

  const toggleExpanded = () => {
    setIsExpanded(!isExpanded);
  };

  return (
    <div className="playlist-card">
      <div className="playlist-header" onClick={toggleExpanded}>
        <h2 className="playlist-title">{playlistData.title}</h2>
        <span className="toggle-icon">{isExpanded ? '▼' : '►'}</span>
      </div>

      <button onClick={() => onEdit(playlistData)} className="edit">Edit</button>
      <button onClick={() => onDelete(playlistData.guid)} className='delete'>Delete</button>
      {isExpanded && (
        <div className="songs-container">
          {playlistData.songs && playlistData.songs.length > 0 ? (
            playlistData.songs.map((song, index) => (
              <SongItem
                key={index}
                song={song}
                onEdit={() => onSongEdit(song)}
                onDelete={() => onSongDelete(song)}
              />
            ))
          ) : (
            <p>No songs in this playlist.</p>
          )}
        </div>
      )}
    </div>
  );
}

export default Playlist;
