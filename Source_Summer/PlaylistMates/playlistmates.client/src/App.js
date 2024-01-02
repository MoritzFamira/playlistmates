import React, { useState, useEffect } from 'react';
import Playlist from './components/Playlist';
import EditModal from './components/EditModal';
import './App.css';


function App() {
    const [playlists, setPlaylists] = useState([]);
    const [filter, setFilter] = useState('');
    const [currentPage, setCurrentPage] = useState(1);
    const [itemsPerPage] = useState(10);
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [currentPlaylistForEdit, setCurrentPlaylistForEdit] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);


  const fetchInitialPlaylists = async () => {
    setIsLoading(true);
    try {
        const response = await fetch(`https://localhost:7227/api/PlaylistControllerd/page/1`, {
            method: 'POST',
            headers: { 'Accept': 'application/json' },
            body: JSON.stringify({}),
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const initialPlaylists = await response.json();
        setPlaylists(initialPlaylists);
        setHasMore(initialPlaylists.length === itemsPerPage);
        setCurrentPage(2);
    } catch (error) {
        console.error('Error fetching initial playlists:', error);
    } finally {
        setIsLoading(false);
    }
};

useEffect(() => { 
  fetchInitialPlaylists(); // eslint-disable-next-line
}, []); // Empty dependency array to ensure it runs only once






  const fetchPlaylists = async () => {
    if (!hasMore || isLoading) return;

    setIsLoading(true);

    try {
        const response = await fetch(`https://localhost:7227/api/PlaylistControllerd/page/${currentPage}`, {
            method: 'POST',
            headers: { 'Accept': 'application/json' },
            body: JSON.stringify({}),
        });
        const newPlaylists = await response.json();

        setPlaylists(prevPlaylists => [...prevPlaylists, ...newPlaylists]);
        setHasMore(newPlaylists.length > 0);
        setCurrentPage(prevPage => prevPage + 1);
    } catch (error) {
        console.error('Error fetching more playlists:', error);
    } finally {
        setIsLoading(false);
    }
};




 const handleEdit = (playlist) => {
  if (playlist) {
    setCurrentPlaylistForEdit({ ...playlist });
  } else {
    setCurrentPlaylistForEdit({ title: '', songs: [] });
  }
  setIsEditModalOpen(true);
};
  

    const handleModalClose = () => {
        setIsEditModalOpen(false);
    };

    
    const handleSaveChanges = async (updatedPlaylist) => {
      try {
        const response = await fetch(`https://localhost:7227/api/PlaylistControllerd/update/${updatedPlaylist.guid}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(updatedPlaylist),
        });
        if (response.ok) {
          // Update local state to reflect changes
          const updatedPlaylists = playlists.map(p => 
            p.guid === updatedPlaylist.guid ? updatedPlaylist : p
          );
          setPlaylists(updatedPlaylists);
        } else {
          console.error('Failed to update playlist');
        }
      } catch (error) {
        console.error('Error updating playlist:', error);
      }
      setIsEditModalOpen(false);
    };
    
  
    
    const handleFilterChange = async (event) => {
      const newFilter = event.target.value.toLowerCase();
      setFilter(newFilter);
      try {
        const response = await fetch(`https://localhost:7227/api/PlaylistControllerd/filter/${newFilter}`);
        const filteredData = await response.json();
        setPlaylists(filteredData);
      } catch (error) {
        console.error('Error fetching filtered playlists:', error);
      }
    };
    
    

    const handleDelete = async (playlistGuid) => {
      try {
        const response = await fetch(`https://localhost:7227/api/PlaylistControllerd/delete/${playlistGuid}`, { method: 'DELETE' });
        if (response.ok) {
          // Remove deleted playlist from local state
          setPlaylists(playlists.filter(p => p.guid !== playlistGuid));
        } else {
          console.error('Failed to delete playlist');
        }
      } catch (error) {
        console.error('Error deleting playlist:', error);
      }
    };
    


    // const handleUpdate = async (playlistGuid, updatedName) => {
    //     try {
    //         const updatedData = { title: updatedName };
    //         await fetch(`https://localhost:7227/api/Playlist/${playlistGuid}`, {
    //             method: 'PUT',
    //             headers: { 'Content-Type': 'application/json' },
    //             body: JSON.stringify(updatedData),
    //         });
    //         setPlaylists(playlists.map(playlist =>
    //             playlist.guid === playlistGuid ? { ...playlist, title: updatedName } : playlist
    //         ));
    //     } catch (error) {
    //         console.error('Error updating playlist:', error);
    //     }
    // };

    const filteredPlaylists = playlists.filter(playlist => filter === 'all' || filter === '' || playlist.category === filter);

    const handleAdd = async () => {
      const newPlaylist = { title: 'New Playlist', description: 'empty', isPublic: true };
      try {
          await fetch('https://localhost:7227/api/PlaylistControllerd/create', {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify(newPlaylist),
          });
          // Fetch the playlists again to update the state with the new playlist
          await fetchInitialPlaylists();
      } catch (error) {
          console.error('Error adding new playlist:', error);
      }
  };
  
  
    

    

    return (
        <div className="App">
            <EditModal
              key={currentPlaylistForEdit ? currentPlaylistForEdit.guid : 'edit-modal'}
              isOpen={isEditModalOpen}
              onClose={handleModalClose}
              playlist={currentPlaylistForEdit}
              onSave={handleSaveChanges}
            />

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
                    {filteredPlaylists.map(playlist => (
                        <Playlist
                        key={playlist.guid}
                        onDelete={handleDelete}
                        onEdit={handleEdit}
                        playlistData={playlist}
                      />
                    ))}
                </div>
                {isLoading && <div>Loading more playlists...</div>}
                {!isLoading && hasMore && (
                <button onClick={fetchPlaylists} className="load-more-button">Load More</button>
            )}
            </main>
            <button onClick={() => handleAdd({titel: 'New Playlist' })}>
                Add Playlist
            </button>
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
