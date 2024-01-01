import React from 'react';

function SongItem({ song}) {
  const artists = song.artists ? song.artists.join(', ') : 'Unknown Artist';

  return (
    <div className="song-item">
      <p>{song.titel} by {artists}</p>

    </div>
  );
}

export default SongItem;
