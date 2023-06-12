import { createTheme, ThemeProvider } from '@mui/material/styles';
import PlaylistList from './components/PlayList';
import { useState } from "react";

const defaultTheme = createTheme();
async function getSongs() {

    var myHeaders = new Headers();
    myHeaders.append("Authorization", "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiSnVzdG9uX1ZvbkBnbWFpbC5jb20iLCJuYmYiOjE2ODY2MDk5MDksImV4cCI6MTY4NzIxNDcwOSwiaWF0IjoxNjg2NjA5OTA5fQ.zaWUzSthhvUx0HkAylYCExU3YgNx0fEbaY78hNXnfM4");
            
    var requestOptions = {
      method: 'GET',
      headers: myHeaders,
      redirect: 'follow'
    };
    let songs = []
    await fetch("http://localhost:5054/api/Playlist/byUser/Juston_Von@gmail.com", requestOptions)
      .then(response => response.json())
      .then(result => {
        songs = result[0].songs
        })
      .catch(error => console.log('error', error));
      console.log(songs)
    return songs;
}
export default async function Test() {
    const [songs, setSongs] = useState([])
    
    
    return (
        
        <PlaylistList role={"OWNER"} songs={[{
                "guid": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
                "isrcCode": "string",
                "titel": "string",
                "releaseDate": "2023-06-12T20:17:28.923Z",
                "durationInMillis": 0
            },{
                "guid": "3fb85f64-5717-4562-b3fc-2c963f66afa6",
                "isrcCode": "string",
                "titel": "string",
                "releaseDate": "2023-06-12T20:17:28.923Z",
                "durationInMillis": 0
            }]}></PlaylistList>
    );
}