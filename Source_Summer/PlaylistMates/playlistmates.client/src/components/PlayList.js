import List from '@mui/material/List';
import ListItemButton from '@mui/material/ListItemButton';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';

/**
 * songs is an array of objects that look like this:
 * {
  "guid": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "isrcCode": "string",
  "titel": "string",
  "releaseDate": "2023-06-12T20:17:28.923Z",
  "durationInMillis": 0,
  "artists": [
    {
      "name": "string"
    }
  ],
  "songCollectionGuids": [
    "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ]
}
 **/

function PlaylistList({ songs, role }) {
  console.log("songs")
  console.log(songs)
  return (<><List>
    {songs === undefined ? (
      <></>
    ) : (
      songs.map((song) => {
        return (

          <ListItem key={song.guid} disablePadding>
            <ListItemText primary={song.titel} />
            {(role === "OWNER")?(<ListItemButton>
              Delete
            </ListItemButton>):(<></>) }
            
          </ListItem>
        )
      })
    )}
  </List>
  </>
  );
}

export default PlaylistList;