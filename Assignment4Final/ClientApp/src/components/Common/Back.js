import { Button } from "react-bootstrap"
import { useNavigate } from "react-router-dom"


export default function BackButton() {
    const navigate=useNavigate();
    return (
        <Button variant='secondary' className='d-grid gap-2 col-12 mx-auto py-2 my-2' onClick={() => navigate(-1)}>Go back</Button>
    )
}
