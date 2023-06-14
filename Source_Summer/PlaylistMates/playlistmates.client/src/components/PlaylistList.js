import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { List, ListItem, ListItemButton, ListItemText } from "@mui/material";

const PlaylistList = () => {
  const { playlistId } = useParams();
  const [playlist, setPlaylist] = useState(null);
  const [role, setRole] = useState("");
  useEffect(() => {
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

      await fetch(
        `${process.env.REACT_APP_API_URL}/api/Playlist/` + playlistId,
        requestOptions
      )
        .then((response) => response.json())
        .then((result) => setPlaylist(result))
        .catch((error) => console.log("error", error));
    };

    fetchPlaylist();
  }, [playlistId]); // fetchPlaylist will only run when playlistId changes

  if (!playlist) {
    return <div>Loading...</div>; // render loading state
  }
  console.log(playlist);
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
  
  const fetchRole = async () => {
    await fetch("${process.env.REACT_APP_API_URL}/api/Playlist/4/role", requestOptions)
      .then((response) => response.text())
      .then((result) => (setRole(result)))
      .catch((error) => console.log("error", error));
    console.log(role);
  };
  fetchRole();

  return (
    <List>
      {playlist.songs.map((song) => (
        <ListItem key={song.id}>
          <ListItemText>{song.titel}</ListItemText>
          {role === "LISTENER" || role === "OWNER" ? (
            <ListItemButton>Delete</ListItemButton>
          ) : (
            <></>
          )}
        </ListItem>
      ))}
    </List>
  );
};

export default PlaylistList;
