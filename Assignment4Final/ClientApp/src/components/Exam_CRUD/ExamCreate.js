import React, { useEffect, useState, useContext } from "react";

import { ListGroup, ListGroupItem, Button, Table, Row, Col, Stack, Form, CloseButton } from 'react-bootstrap';
import { AuthenticationContext } from '../auth/AuthenticationContext'
import { getUserId } from '../auth/handleJWT'

import { useNavigate, useParams } from "react-router-dom";
import DatePicker from "react-datepicker";
import Register from "../auth/Register";
import "react-datepicker/dist/react-datepicker.css";
import axios from "axios";



// export default function ExamCreate() {

//     const [exam , setExam ] = useExam({
        
//     });

//     setExam({...exam,certificate : cert})

// }