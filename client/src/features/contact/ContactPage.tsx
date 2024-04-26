import React, { useState } from 'react';
import { Button, Container, TextField, Typography } from '@mui/material';

interface FormValues {
  name: string;
  email: string;
  message: string;
}

const ContactPage: React.FC = () => {
  const [values, setValues] = useState<FormValues>({
    name: '',
    email: '',
    message: ''
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setValues(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    console.log('Form submitted with values:', values);
    // Add your form submission logic here
  };

  return (
    <Container maxWidth="md">
      <Typography variant="h2" gutterBottom>Contact Us</Typography>
      <form onSubmit={handleSubmit}>
        <TextField
          fullWidth
          margin="normal"
          id="name"
          name="name"
          label="Name"
          value={values.name}
          onChange={handleChange}
        />
        <TextField
          fullWidth
          margin="normal"
          id="email"
          name="email"
          label="Email"
          type="email"
          value={values.email}
          onChange={handleChange}
        />
        <TextField
          fullWidth
          margin="normal"
          id="message"
          name="message"
          label="Message"
          multiline
          rows={4}
          value={values.message}
          onChange={handleChange}
        />
        <Button type="submit" variant="contained" color="primary">Submit</Button>
      </form>
    </Container>
  );
}

export default ContactPage;