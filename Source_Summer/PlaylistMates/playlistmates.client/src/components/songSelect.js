import React, { useState, useEffect } from 'react';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';

async function getSongs() {

    const response = await fetch("https://localhost:7227/api/Song", {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "authorization": "bearer " + localStorage.getItem("jwtToken")
        },
    });
    const data = await response.json();
    console.log(data);
    const songs = data.map((song) => {
        return {
            guid: song.guid,
            titel: song.titel,
        };
    });

    return songs;
}

function SongSelect() {
    //const [options, setOptions] = useState([]);
    const [songs, setSongs] = useState([]);

    useEffect(() => {
        const fetchSongs = async () => {
            const songs = await getSongs();

            setSongs(songs);
            //setOptions(songs.map((song) => song.titel));
        };

        fetchSongs();
    }, []);

    if (songs.length === 0) {
        return null; // or render a loading state if desired
    }

    return (
        <Autocomplete
            disablePortal
            id="combo-box-demo"
            options={songs}
            getOptionLabel={(song) => song.titel}
            sx={{ width: 300 }}
            renderInput={(params) => <TextField {...params} label="add Song" />}
        />
    );
}

export default SongSelect;
