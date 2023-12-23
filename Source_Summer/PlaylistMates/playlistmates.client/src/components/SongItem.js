import React from 'react';

function SongItem({ song, onEdit, onDelete }) {
  return (
    <div className="song-item">
      <p>{song.title} by {song.artist}</p>
      <button onClick={() => onEdit(song)} className="edit">Edit</button>
      <button onClick={() => onDelete(song)} className='delete'>Delete</button>
    </div>
  );
}

export default SongItem;
