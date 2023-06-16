import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { List, ListItem, ListItemButton, ListItemText } from "@mui/material";
import ConfirmationDialogue from './components/ConfirmationDialogue';
import SongSelect from './components/songSelect.js';
import BackButton from "./components/BackButton.js"


const PlaylistList = () => {
  const { playlistId } = useParams();
  const [playlist, setPlaylist] = useState(null);
  const [role, setRole] = useState("");
  const [isConfirmationOpen, setIsConfirmationOpen] = useState(false);
  const [songIdToDelete, setSongIdToDelete] = useState(null);


  const fetchPlaylist = async () => {
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
        `${process.env.REACT_APP_API_URL}/api/Playlist/` + playlistId,
        requestOptions
      );
      const result = await response.json();
      setPlaylist(result);
    } catch (error) {
      console.log("error", error);
    }
  };

  useEffect(() => {
    fetchPlaylist();
  }, [playlistId]);

  useEffect(() => {
    const fetchRole = async () => {
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
          `${process.env.REACT_APP_API_URL}/api/Playlist/4/role`,
          requestOptions
        );
        const result = await response.text();
        setRole(result);
        console.log(result);
      } catch (error) {
        console.log("error", error);
      }
    };

    fetchRole();
  }, []);

  const deleteSong = async (songId) => {
    var myHeaders = new Headers();
    myHeaders.append(
      "Authorization",
      "Bearer " + localStorage.getItem("jwtToken")
    );

    var requestOptions = {
      method: "DELETE",
      headers: myHeaders,
    };

    try {
      const response = await fetch(
        `${process.env.REACT_APP_API_URL}/api/Playlist/${playlistId}/songs/${songId}`,
        requestOptions
      );

      if (!response.ok) {
        throw new Error(
          "There was an error deleting the song. We apologize for the inconvenience."
        );
      }

      fetchPlaylist();
    } catch (error) {
      console.log("error", error);
    }
  };

  const removeSong = (songId) => {
    setIsConfirmationOpen(true);
    setSongIdToDelete(songId);
  };

  if (!playlist) {
    return <div>Loading...</div>;
  }

  return (
    <>

      <div style={
        {
          position: "fixed",
          top: "10px",
          left: "10px",
          zIndex: "1000",
          color: "red"
        }
      }>
        <BackButton />
      </div>
      <h1>{playlist.title}</h1>
      <h4>{playlist.description}</h4>
      <List>
        {playlist.songs.map((song) => (
          <ListItem key={song.id}>
            <ListItemText>{song.titel}</ListItemText>
            {(role === "LISTENER" || role === "OWNER") && (
              <ListItemButton onClick={() => removeSong(song.id)}>
                Delete
              </ListItemButton>
            )}
          </ListItem>
        ))}
        <ConfirmationDialogue
          open={isConfirmationOpen}
          setOpen={setIsConfirmationOpen}
          deleteSong={deleteSong}
          songIdToDelete={songIdToDelete}
        />
      </List>
      <SongSelect></SongSelect>
    </>
  );
};

export default PlaylistList;
