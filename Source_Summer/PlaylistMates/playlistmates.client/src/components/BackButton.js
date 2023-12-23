import { IconButton } from '@mui/material'
import React from 'react'
import ArrowBackRoundedIcon from '@mui/icons-material/ArrowBackRounded';
import { useNavigate } from "react-router-dom";

export default function BackButton() {
  const navigate = useNavigate();
    function onClick() {
        navigate("/playlists");
    }
  return (
    <div>
      <IconButton size="large" onClick={onClick}>
        <ArrowBackRoundedIcon />
      </IconButton>
    </div>
  );
}