import React, {Component} from "react";
import './App.css';
import NavBar from './components/NavBar';
import ReviewForm from './components/ReviewForm';
import OneToFive from './components/OneToFive';
import SubmitButton from './components/SubmitButton';

import 'bootstrap/dist/css/bootstrap.min.css';

import {Container, Row, Col, Button, Form, InputGroup, ButtonGroup} from 'react-bootstrap';



function App(){
  return(
    <div className ="App">
      <NavBar />
      <header className = "App-header">
        <Container>
            <Form>
              <Row>
                <OneToFive />
                <ReviewForm />
                <SubmitButton />
              </Row>
            </Form>
        </Container> 
      </header>     
    </div>
  );
}

export default App;