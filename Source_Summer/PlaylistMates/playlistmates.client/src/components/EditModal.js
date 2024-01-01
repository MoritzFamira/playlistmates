import React, { useState, useEffect } from 'react';
import '../EditModal.css'; 

const availableSongs = [
  { titel: "Song 1", artists: ["Author 1"] },
  { titel: "Song 2", artists: ["Author 2"] },
];

function EditModal({ isOpen, onClose, playlist, onSave }) {
    const [updatedPlaylist, setUpdatedPlaylist] = useState(playlist || { title: '', songs: [] });
    const [selectedSongIndex, setSelectedSongIndex] = useState(0);
    const [showSongDropdown, setShowSongDropdown] = useState(false);

  useEffect(() => {
    if (playlist) {
      setUpdatedPlaylist({ ...playlist });
    }
  }, [playlist]);
  
  console.log(playlist);
  if (!isOpen) return null;

  const handleSongDelete = (index) => {
    const newSongs = updatedPlaylist.songs.filter((_, songIndex) => songIndex !== index);
    setUpdatedPlaylist({ ...updatedPlaylist, songs: newSongs });
  };

  const toggleSongDropdown = () => {
    setShowSongDropdown(!showSongDropdown);
  };

  const handleAddSong = () => {
    const songToAdd = availableSongs[selectedSongIndex];
    // Ensure songs array is initialized
    const songsArray = updatedPlaylist.songs || [];
    setUpdatedPlaylist({ ...updatedPlaylist, songs: [...songsArray, songToAdd] });
    setShowSongDropdown(false); // Hide dropdown after adding a song
};



  return (
    <div className="modal">
      <div className="modal-content">
        <div className="modal-header">
          <span className="close" onClick={onClose}>&times;</span>
          <h2>Edit Playlist</h2>
          <input
            className="modal-input"
            type="text"
            name="title"
            value={updatedPlaylist.title}
            onChange={(e) => setUpdatedPlaylist({ ...updatedPlaylist, title: e.target.value })}
          />
        </div>
        <div className="song-list">
          {updatedPlaylist.songs != null && updatedPlaylist.songs.length > 0 ? (
            updatedPlaylist.songs.map((song, index) => (
              <div key={index} className="song-item">
                <span>{song.titel} by {song.artists.join(', ')}</span>                
                <div className="song-controls">
                  <button className="control-button" onClick={() => handleSongDelete(index)}>Delete</button>
                </div>
              </div>
            ))
          ) : (
            <p className="no-songs-text">No songs...</p>
          )}
        </div>
        {showSongDropdown && (
          <div>
            <select
              value={selectedSongIndex}
              onChange={(e) => setSelectedSongIndex(e.target.value)}
            >
              {availableSongs.map((song, index) => (
                <option key={index} value={index}>
                  {song.titel}
                </option>
              ))}
            </select>
            <button className="control-button" onClick={handleAddSong}>Add Song</button>
          </div>
        )}
        <button className="add-song-button" onClick={toggleSongDropdown}>Add New Song</button>
        <button className="control-button" onClick={() => onSave(updatedPlaylist)}>Save Changes</button>
      </div>
    </div>
  );
}

export default EditModal;
