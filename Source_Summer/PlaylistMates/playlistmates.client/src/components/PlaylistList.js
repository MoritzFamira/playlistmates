import { useEffect, useState } from "react";
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

    await fetch(
      "http://localhost:5054/api/Playlist/" + playlistId,
      requestOptions
    )
      .then((response) => response.json())
      .then((result) => setPlaylist(result))
      .catch((error) => console.log("error", error));
  };
  
  useEffect(() => {

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
    await fetch("http://localhost:5054/api/Playlist/4/role", requestOptions)
      .then((response) => response.text())
      .then((result) => (setRole(result)))
      .catch((error) => console.log("error", error));
    console.log(role);
  };
  fetchRole();

  async function removeSong(songId) {
    var myHeaders = new Headers();
    myHeaders.append(
      "Authorization",
      "Bearer " + localStorage.getItem("jwtToken")
    );

    var requestOptions = {
      method: "DELETE",
      headers: myHeaders,
    };

    await fetch(
      "http://localhost:5054/api/Playlist/" + playlistId + "/songs/" + songId,
      requestOptions
    )
      .then((response) => {
        console.log(response.text());
        if (!response.ok) {
          if (response.status === 204) {
            fetchPlaylist();
          } else {
            throw new Error("There was an error deleting the song, we apologize for the inconvenience");
          }
        }
      })
      .catch((error) => console.log("error", error));
      
  };
    

  return (
    <List>
      {playlist.songs.map((song) => (
        <ListItem key={song.id}>
          <ListItemText>{song.titel}</ListItemText>
          {role === "LISTENER" || role === "OWNER" ? (
            <ListItemButton onClick={() => {removeSong(song.id)}}>Delete</ListItemButton>
          ) : (
            <></>
          )}
        </ListItem>
      ))}
    </List>
  );
};

export default PlaylistList;
