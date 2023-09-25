import React from 'react';
import ReactDOM from 'react-dom';
import { useLocation, useNavigate } from "react-router-dom";
import { Button, Navbar, Card, Stack, Table } from 'react-bootstrap';
import BackButton from '../Common/Back'


import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { Row, Col } from 'antd';

import logocolor from '../logo/logocolor.png';

console.log(logocolor);


export default function Results() {
    const location = useLocation();
    const navigate = useNavigate();
    const incomingData = location.state.data;
    console.log(incomingData);
    // - - - -  - - - - - - - - - - - - - - - - - - -
    console.log(incomingData.candidate);
    console.log(incomingData.candidate.firstName);
    console.log(incomingData.examDate);
    // var result = incomingData.result ? "Passed" : "Failed";
    //---------------------------------------------------------------------------------Date and Time
    var timeResult = incomingData.examDate.toString();
    var date = timeResult.slice(0, 10);
    var hour = timeResult.slice(11, 16);
    console.log(hour);
    //---------------------------------------------------------------------------------
    // ----------------------------------------------------------------------Create Document Component
    const handleClick = () => {
        //----------------------PDF-------------------------------------------->>>>>>>>>>>
        var doc = new jsPDF();
        var element = document.getElementById('jsPdf');
        html2canvas(element).then(canvas => {
            var imgData = canvas.toDataURL("image/jpeg", 10.0);
            // addImage(imageData, format, x, y, width, height, alias, compression, rotation)
            doc.addImage(imgData, 'BMP', 15, 15, 180, 0, "someth", "SLOW", 0);
            doc.setLineWidth(0.2);
            doc.rect(15, 10, 180, 200);
            doc.save('MyResults.pdf');
        });

    }
    return (
        <div >
            <h1 class="display-3 text-center align-middle">Examination Result</h1>
            {/* ------------------------------------------------------------------------------------------------------ */}
            <div className='container-fluid' id='jsPdf'>
                <Row className='justify-content-center'>
                    <img src={logocolor} alt='some logo' width={200} height={200} id='resultLogo'/>
                </Row>
                <hr />
                {/* <Table>
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Exam Date</th>
                            <th>Max Score</th>
                            <th>Passing Score</th>
                            <th>My Score</th>
                            <th>My Percentage Score</th>
                            <th>Result</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>{incomingData.exam.certificateTitle}</td>
                            <td>{incomingData.examDate}</td>
                            <td>{incomingData.maxScore}</td>
                            <td>{incomingData.exam.passMark}</td>
                            <td>{incomingData.candidateScore}</td>
                            <td>{incomingData.percentScore} %</td>
                            <td style={{ color: incomingData.result ? 'green' : 'red' }}>
                                {incomingData.result ? "Passed" : "Failed"}
                            </td>
                        </tr>
                    </tbody>
                </Table> */}
                <Card bg='light'>
                    <Card.Header>
                        <h4>Results for the Candidate : {incomingData.candidate.firstName} {incomingData.candidate.lastName}</h4>
                    </Card.Header>
                    <Card.Body>
                        <Table bordered >
                            <thead>
                                <tr>
                                    <th>Exam Title</th>
                                    <th>Exam Date</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>{incomingData.exam.certificateTitle}</td>
                                    <td>{date},{hour} </td>
                                </tr>
                            </tbody>
                        </Table>

                        <Table    >
                            <thead>
                                <tr>
                                    <th>My Score</th>
                                    <th>Passing Score</th>
                                    <th>Max Score</th>
                                    <th>My Percentage Score</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>{incomingData.candidateScore}</td>
                                    <td>{incomingData.exam.passMark}</td>
                                    <td>{incomingData.maxScore}</td>
                                    <td>{incomingData.percentScore} %</td>
                                </tr>
                            </tbody>
                        </Table>
                        <Row>Examination Result : {incomingData.result ? "Passed" : "Failed"}</Row>
                        <Row> <i>{incomingData.result ? "Congratulations!!!" : "Thank you for your try."}</i></Row>
                        <hr />

                    </Card.Body>
                </Card>


                {/* ------------------------------------------------------------------------------------------------------ */}
            </div>
            <hr />
            <Button className='d-grid gap-2 col-12 mx-auto py-2 my-2' onClick={handleClick}>Download Results</Button>
            {/* {location.state && location.state.from === '/candidate/availableexams' && (
            )} */}
            <Button variant='secondary' className='d-grid gap-2 col-12 mx-auto py-2 my-2' onClick={() => navigate("/candidate/availableexams")}>Go back</Button>
                {/* <BackButton /> */}
        </div>
    )



}
