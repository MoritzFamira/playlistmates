import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { List, ListItem, ListItemButton, ListItemText } from "@mui/material";

const PlaylistList = () => {
  const { playlistId } = useParams();
  const [playlist, setPlaylist] = useState(null);
  const [role, setRole] = useState("");

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

  const removeSong = async (songId) => {
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

      // fetch playlist again after song removal regardless of response status
      fetchPlaylist();
    } catch (error) {
      console.log("error", error);
    }
  };


  if (!playlist) {
    return <div>Loading...</div>; // render loading state
  }

  return (
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
    </List>
  );
};

export default PlaylistList;
