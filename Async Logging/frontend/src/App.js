import React from "react";
import './App.css';
import NavBar from './components/NavBar';
import ReviewsAndRatings from './components/ReviewsAndRatings';

import 'bootstrap/dist/css/bootstrap.min.css';

// import {Container, Row, Col, Button, Form, InputGroup, ButtonGroup} from 'react-bootstrap';

function App(){
  return(
    <div className ="App">
      <NavBar />
      <header className = "App-header">
      <ReviewsAndRatings />
      </header>     
    </div>
  );
}

export default App;