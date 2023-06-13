import React, { useState } from 'react';
import EditForm from './components/EditForm';
import { Button } from '@mui/material';

const EditTest = () => {
    const [showForm, setShowForm] = useState(false);

    const handleClick = () => {
        setShowForm(true);
    }

    return (
        <div>
            <h1>edit test</h1>
            <Button variant="outlined" color="primary" onClick={handleClick}>
                Show Form
            </Button>
            {showForm && <EditForm />}
        </div>
    );
}

export default EditTest;
