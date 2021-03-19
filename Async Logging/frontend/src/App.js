import React from "react";
import './App.css';
import NavBar from './components/NavBar';
import 'bootstrap/dist/css/bootstrap.min.css';

import {Container,Row,Col, Button, Form, InputGroup} from 'react-bootstrap';

function App(){
  return(
    <div className ="App">
      <NavBar />
      <header className = "App-header">
        <Container>
          <Form>
            <Row>
              <Col>
                <Form.Group controlId="WriteReview">
                  <InputGroup.Prepend>
                    <InputGroup.Text>Enter a Review</InputGroup.Text>
                  </InputGroup.Prepend>
                  <Form.Control as="textarea" aria-label="With textarea" />
                </Form.Group>
              </Col>
              <Col>
                <Form.Group controlId="Submit">
                  <Button type="submit">Submit Review</Button>
                </Form.Group>
              </Col>
            </Row>
          </Form>
        </Container> 
        {/* <Form>
          <Form.Group>
            <Form.Label>Review</Form.Label>
            <Form.Control type="email" placeholder="hello@gmail.com" />
            <Form.Text className="text-muted">We wont share your email</Form.Text>
          </Form.Group>
        </Form> */}
      </header>     
    </div>
  );
}

export default App;