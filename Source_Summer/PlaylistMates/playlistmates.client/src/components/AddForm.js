import React, { useState } from 'react';
import { TextField, Button, Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material';


const AddForm = ({setIsSubmitted}) => {
    const [open, setOpen] = useState(false);
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [errors, setErrors] = useState({ title: '', description: '' });

    const validateForm = () => {
        let tempErrors = { title: '', description: '' };
        let formIsValid = true;

        if(!title || title.trim() === "") {
            formIsValid = false;
            tempErrors.title = 'Title is required';
        }
        
        if(!description || description.trim() === "") {
            formIsValid = false;
            tempErrors.description = 'Description is required';
        }

        setErrors(tempErrors);
        return formIsValid;
    }


    const handleSubmit = async (event) => {
        event.preventDefault();
        //console.log(playlistId)
        if(validateForm()) {
            try {
               
                const token = localStorage.getItem('jwtToken');
                const response = await fetch(`${process.env.REACT_APP_API_URL}/api/Playlist`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + token,
                    },
                    body: JSON.stringify({ title, description })
                });
    
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
    
                const data = await response.text();
                console.log(data);

            } catch (error) {
                console.error('There was a problem with the fetch operation: ', error);
            }
        }
        setIsSubmitted(true);
        setOpen(false);
    }
    return (
        <div>
            <Button variant="outlined" color="primary" onClick={() => setOpen(true)}>
                Add Playlist
            </Button>
            <Dialog open={open} onClose={() => setOpen(false)}>
                <DialogTitle>Add Form</DialogTitle>
                <DialogContent>
                    <form onSubmit={handleSubmit}>
                        <TextField
                            id="title"
                            label="Title"
                            value={title}
                            onChange={e => setTitle(e.target.value)}
                            error={!!errors.title}
                            helperText={errors.title}
                            fullWidth
                            margin="normal"
                        />

                        <TextField
                            id="description"
                            label="Description"
                            value={description}
                            onChange={e => setDescription(e.target.value)}
                            error={!!errors.description}
                            helperText={errors.description}
                            fullWidth
                            margin="normal"
                        />

                        <DialogActions>
                            <Button onClick={() => setOpen(false)} color="secondary">
                                Cancel
                            </Button>
                            <Button type="submit" variant="contained" color="primary">
                                Submit
                            </Button>
                        </DialogActions>
                    </form>
                </DialogContent>
            </Dialog>
        </div>
    );
}

export default AddForm;