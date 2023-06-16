import React, { useState, useEffect } from "react";
import { List, ListItem, ListItemButton, ListItemText } from "@mui/material";
import { useNavigate } from "react-router-dom";
import EditForm from "./components/EditForm";
import CreateForm from "./components/CreateForm";


const Playlists = () => {
  const [playlists, setPlaylists] = useState([]);
  const [isSubmitted, setIsSubmitted] = useState(false);
  const [isCreated, setIsCreated] = useState(false);

  const navigate = useNavigate();

  const fetchPlaylists = async () => {
    var myHeaders = new Headers();
    myHeaders.append(
      "Authorization",
      "Bearer " + localStorage.getItem("jwtToken")
    );

    var requestOptions = {
      method: "GET",
      headers: myHeaders,
      redirect: "follow",
    };

    try {
      const response = await fetch(
        `${process.env.REACT_APP_API_URL}/api/Playlist/byUser/` + localStorage.getItem("email"),
        requestOptions
      );
      const result = await response.json();
      setPlaylists(result);
    } catch (error) {
    }
  };



  useEffect(() => {
    fetchPlaylists();
  }, []);
   if (isSubmitted) {
     setIsSubmitted(false);
     fetchPlaylists();
   }
   if (isCreated) {
     setIsCreated(false);
     fetchPlaylists();
   }
  //console.log(playlists);
  return (
    <List component="nav">
      {playlists.map((playlist, index) => (
        <>
          <ListItem key={index}>
            <ListItemButton
              onClick={() => navigate(`/playlist/${playlist.id}`)}
            >
              <ListItemText primary={playlist.title} />
            </ListItemButton>
          </ListItem>
          <EditForm playlistId={playlist.id} setIsSubmitted={setIsSubmitted} />
          <CreateForm setIsCreated={setIsCreated} /> 
        </>
      ))}
    </List>
  );
};

export default Playlists;
