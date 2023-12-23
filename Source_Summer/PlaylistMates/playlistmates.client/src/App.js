import React, { useState, useEffect } from 'react';
import Playlist from './components/Playlist';
import './App.css';

function App() {
  const [playlists, setPlaylists] = useState([]);
  const [filter, setFilter] = useState('');

  useEffect(() => {
    // Simulating an API call to fetch playlists
    const fetchPlaylists = async () => {
      // Replace with your actual API call
      // const response = await fetch('/api/playlists');
      // const data = await response.json();
      const data = [
        { id: 1, name: "Chill Vibes", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
        // ... more playlists
      ];
      setPlaylists(data);
    };

    fetchPlaylists();
  }, []);

  const handleFilterChange = (event) => {
    setFilter(event.target.value.toLowerCase());
    // Simulating an API call to fetch filtered playlists
    // Fetch data from the API with the new filter
    // const fetchFilteredPlaylists = async () => {
    //   const response = await fetch(`/api/playlists?filter=${filter}`);
    //   const data = await response.json();
    //   setPlaylists(data);
    // };
    // fetchFilteredPlaylists();
  };


  const handleDelete = async (playlistId) => {
    // Simulate API call to delete a playlist
    // await fetch(`/api/playlists/delete/${playlistId}`, { method: 'DELETE' });
    setPlaylists(playlists.filter(playlist => playlist.id !== playlistId));
  };

  const handleUpdate = async (playlistId, updatedData) => {
    // Simulate API call to update a playlist
    // await fetch(`/api/playlists/update/${playlistId}`, { method: 'PUT', body: JSON.stringify(updatedData) });
    setPlaylists(playlists.map(playlist => playlist.id === playlistId ? updatedData : playlist));
  };

  const handleAdd = async (newPlaylist) => {
    // Simulate API call to add a new playlist
    // await fetch('/api/playlists/add', { method: 'POST', body: JSON.stringify(newPlaylist) });
    setPlaylists([...playlists, newPlaylist]);
  };

  const filteredPlaylists = playlists.filter(playlist => {
    if (filter === 'all' || filter === '') return true;
    // Add more filtering logic here based on other criteria
  });
  


  return (
    <div className="App">
      <header className="App-header">
        <h1>PlaylistMates</h1>
        <select onChange={handleFilterChange}>
          <option value="all">All</option>
          <option value="chill">Chill</option>
          <option value="workout">Workout</option>
          <option value="party">Party</option>
        </select>
      </header>
        <main className="App-content">
        {filteredPlaylists.map(playlist => (
              <Playlist key={playlist.id} playlistData={playlist} />
            ))}
        </main>
        <div className="help-button-container">
        <button className="help-button">Need Help?</button>
        <div className="help-image-container">
          <h1 className="help-text">I can't help you, but here is an image of keanu to make you feel better!</h1>
          <img src="keanu.jpg" alt="Help Image" className="help-image" />
        </div>
      </div>
    </div>
  );
}

export default App;