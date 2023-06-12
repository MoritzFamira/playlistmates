import * as React from 'react';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';

var allSongs = []

async function getSongs() {

    var requestOptions = {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
            "authorization": "bearer " + localStorage.getItem("jwtToken")
        },
    };
    await fetch("http://localhost:5054/api/Song", requestOptions)
        .then((response) => response.text())
        .then((data) => (allSongs = data))
        .catch((error) => console.log("error", error));
}

function ComboBox() {
    getSongs()
    console.log(allSongs)
    return (
        <Autocomplete
            disablePortal
            id="combo-box-demo"
            options={allSongs}
            sx={{ width: 300 }}
            renderInput={(params) => <TextField {...params} label="Song" />}
        />
    );
}


export default ComboBox;