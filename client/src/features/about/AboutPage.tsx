import React, { useState } from 'react';
import { Alert, AlertTitle, Container, List, ListItem, ListItemText, Typography } from '@mui/material';
import { GoogleMap, LoadScript, Marker } from '@react-google-maps/api';

const AboutPage: React.FC = () => {
    const [validationErrors] = useState<string[]>([]);


    return (
        <Container>
            <Typography gutterBottom variant={'h2'}>Our Map Location</Typography>
            {validationErrors.length > 0 &&
                <Alert severity="error">
                    <AlertTitle>Validation Errors</AlertTitle>
                    <List>
                        {validationErrors.map(error => (
                            <ListItem key={error}>
                                <ListItemText>{error}</ListItemText>
                            </ListItem>
                        ))}
                    </List>
                </Alert>}
            <LoadScript
                googleMapsApiKey="AIzaSyAmYnavR8DrALwUk_5tYhITQmD5dLkKPS8"
            >
                <GoogleMap
                    mapContainerStyle={{ height: "400px", width: "100%" }}
                    zoom={10}
                    center={{ lat: 51.538891, lng: 0.147430 }}
                >
                    {/* Mark Dagenham with a dot */}
                    <Marker position={{ lat: 51.538891, lng: 0.147430 }} />
                </GoogleMap>
            </LoadScript>
        </Container>
    )
}

export default AboutPage;
