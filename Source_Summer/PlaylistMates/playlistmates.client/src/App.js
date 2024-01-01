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


    //TODO: Uncomment this once the put works
    // const handleEdit = (playlist) => {
    //     setCurrentPlaylistForEdit(playlist);
    //     setIsEditModalOpen(true);
    // };

 const handleEdit = (playlist) => {
  if (playlist) {
    setCurrentPlaylistForEdit({ ...playlist });
  } else {
    // For new playlists, initialize with default values
    setCurrentPlaylistForEdit({ title: '', songs: [] });
  }
  setIsEditModalOpen(true);
};
  

    const handleModalClose = () => {
        setIsEditModalOpen(false);
    };
    //TODO: Uncomment this once the put works
    // const handleSaveChanges = async (updatedPlaylist) => {
    //   try {
    //     await fetch(`https://localhost:7227/api/Playlist/${updatedPlaylist.guid}`, {
    //       method: 'PUT',
    //       headers: { 'Content-Type': 'application/json' },
    //       body: JSON.stringify(updatedPlaylist),
    //     });
    //     setPlaylists(playlists.map(playlist => 
    //       playlist.guid === updatedPlaylist.guid ? updatedPlaylist : playlist
    //     ));
    //   } catch (error) {
    //     console.error('Error updating playlist:', error);
    //   }
    // };

    const handleSaveChanges = (updatedPlaylist) => {
      const updatedPlaylists = playlists.map(p => 
        p.guid === updatedPlaylist.guid ? updatedPlaylist : p
      );
      setPlaylists(updatedPlaylists);
      console.log(updatedPlaylists);
      setIsEditModalOpen(false);
    };
  
    
    const handleFilterChange = async (event) => {
      const newFilter = event.target.value.toLowerCase();
      setFilter(newFilter);
    
      try {
        const response = await fetch(`https://localhost:7227/api/PlaylistControllerd/filter/${newFilter}`);
        const data = await response.json();
        setPlaylists(data);
      } catch (error) {
        console.error('Error fetching filtered playlists:', error);
      }
    };
    

    const handleDelete = async (playlistGuid) => {
      try {
        const response = await fetch(`https://localhost:7227/api/Playlist/${playlistGuid}`, { method: 'DELETE' });
        if (response.ok) {
          fetchPlaylists(currentPage); // Refetch playlists for the current page
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

    const handleAdd = async (newPlaylist) => {
        try {
            await fetch('https://localhost:7227/api/Playlist', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newPlaylist),
            });
            setPlaylists([...playlists, newPlaylist]);
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
            <button onClick={() => handleAdd({ guid: 'new-guid', titel: 'New Playlist' })}>
                Add Playlist
            </button>
        </div>
    );
}

export default App;
