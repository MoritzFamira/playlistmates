import React, { useState, useEffect } from 'react';
import Playlist from './components/Playlist';
import './App.css';
import Pagination from '@mui/material/Pagination';


function App() {
    const [playlists, setPlaylists] = useState([]);
    const [filter, setFilter] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(10);

    useEffect(() => {
      // Simulating an API call to fetch playlists
      const fetchPlaylists = async () => {
        // Replace with your actual API call
        // const response = await fetch('/api/playlists');
        // const data = await response.json();
        const data = [
          { id: 1, title: "Chill Vibes1", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 2, title: "Chill Vibes2", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 3, title: "Chill Vibes3", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 4, title: "Chill Vibes4", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 5, title: "Chill Vibes5", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 6, title: "Chill Vibes6", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 7, title: "Chill Vibes7", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 8, title: "Chill Vibes8", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 9, title: "Chill Vibes9", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 10, title: "Chill Vibes10", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 11, title: "Chill Vibes11", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },
          { id: 12, title: "Chill Vibes12", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },

          { id: 13, title: "Chill Vibes", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },

          { id: 14, title: "Chill Vibes", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },

          { id: 15, title: "Chill Vibes", songs: [{ title: "Song 1", artist: "Artist 1" }, { title: "Song 2", artist: "Artist 2" }] },


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

const indexOfLastPlaylist = currentPage * itemsPerPage;
const indexOfFirstPlaylist = indexOfLastPlaylist - itemsPerPage;
const currentPlaylists = playlists.slice(indexOfFirstPlaylist, indexOfLastPlaylist);


  
  const handlePageChange = (event, value) => {
    console.log('Changing to page: ', value);
    setCurrentPage(value);
  };

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
        <div className="playlists-container">
          {currentPlaylists
          .map(playlist => (
            <Playlist key={playlist.id} playlistData={playlist} />
          ))}
        </div>
      </main>
      <Pagination
        component="div"
        count={Math.ceil(playlists.length / itemsPerPage)}
        page={currentPage}
        onChange={handlePageChange}
        color="primary"
      />
        <div className="help-button-container">
        <button className="help-button">Need help?</button>
        <div className="help-image-container">
          <h1 className="help-text">I can't help you, but here is an image of Tom to make you feel better!</h1>
          <img src="tomhanks.jpg" alt="Help Image" className="help-image" />
        </div>
      </div>
      <div className="helpMore-button-container">
        <button className="helpMore-button">Need more help?</button>
        <div className="helpMore-image-container">
          <h1 className="helpMore-text">I can't help you, but here is an image of Emilia to make you feel better!</h1>
          <img src="emiliaschuele.jpg" alt="Help Image" className="helpMore-image" />
        </div>
      </div>
      <div className="helpMore2-button-container">
        <button className="helpMore2-button">Need extremely much more more more help?</button>
        <div className="helpMore2-image-container">
          <h1 className="helpMore2-text">I can't help you, but here is an image of Keanu to make you feel better!</h1>
          <img src="keanu.jpg" alt="Help Image" className="helpMore2-image" />
        </div>
      </div>
    </div>
  );
}

export default App;