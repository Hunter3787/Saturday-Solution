import React from "react";

import Button from "react-bootstrap/Button";
import Accordion from "react-bootstrap/Accordion";
import Card from "react-bootstrap/Card";

function Builds(props) {
  return (
    <div>
      <Accordion>
        {props.builds.map(function (build) {
          return (
            <Card key={build.id}>
              <Card.Header>
                <Accordion.Toggle
                  as={Button}
                  variant="link"
                  eventKey={build.id}
                >
                  {build.title}
                </Accordion.Toggle>
              </Card.Header>
              <Accordion.Collapse eventKey={build.id}>
                <Card.Body>
                  <ul>
                    {Object.keys(build).map(function (key, index) {
                      if (key == "id") {
                        return;
                      }
                      const buildItem = build[key];
                      return (
                        <li key={key}>
                          {key}: {buildItem}
                        </li>
                      );
                    })}
                  </ul>
                </Card.Body>
              </Accordion.Collapse>
            </Card>
          );
        })}
      </Accordion>
    </div>
  );
}

export default Builds;
