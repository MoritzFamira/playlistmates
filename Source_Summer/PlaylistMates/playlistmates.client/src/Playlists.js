import React, { useState, useEffect } from "react";
import { List, ListItem, ListItemButton, ListItemText } from "@mui/material";
import { useNavigate } from "react-router-dom";

const Playlists = () => {
  const [playlists, setPlaylists] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchPlaylists = async () => {
      const email = localStorage.getItem("email"); // Assuming the user's email is stored in localStorage
      const token = localStorage.getItem("jwtToken"); // Assuming JWT token is stored in localStorage
      console.debug(email, token);
      const response = await fetch(
        `http://localhost:5054/api/Playlist/byUser/${email}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      ).catch((error) => console.log("error", error)); //<Alert severity="error">This is an error alert — check it out!</Alert>

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      } else {
        const data = await response.json();
        setPlaylists(data);
      }
    };

    fetchPlaylists();
  }, []);
  
  return (
    <List component="nav">
      {playlists.map((playlist, index) => (
        <ListItem key={index}>
          <ListItemButton onClick={() => navigate(`/playlist/${playlist.id}`)}>
            <ListItemText primary={playlist.description} />
          </ListItemButton>
        </ListItem>
      ))}
    </List>
  );
};

export default Playlists;
