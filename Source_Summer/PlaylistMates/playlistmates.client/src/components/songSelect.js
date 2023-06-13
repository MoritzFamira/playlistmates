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
    const titles = data.map(item => item.titel);
    return titles;
}

function ComboBox() {
    const [songs, setSongs] = useState([]);

    useEffect(() => {
        const fetchSongs = async () => {
            const titles = await getSongs();
            setSongs(titles);
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
            sx={{ width: 300 }}
            renderInput={(params) => <TextField {...params} label="Song" />}
        />
    );
}

export default ComboBox;
