import { Box, Button, Card, CardActions, CardContent, CardMedia, Icon, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Row } from "react-bootstrap";
import Col from "react-bootstrap/esm/Col";
import "./Home.css";
import EventAvailableIcon from '@mui/icons-material/EventAvailable';
import EventBusyIcon from '@mui/icons-material/EventBusy';
import { JsxElement } from "typescript";

interface ICarCard{
    carId: string,
    brand: string,
    model : string,
    licensePlate: string,
    yearOfIssue: number,
    isAvailable: boolean,
    image: string,
    level: number
}


interface ICarData{
    data: boolean
}


export default function CarsCards(page: number, count: number){
    const [cars, setCar] = useState<ICarCard[]>([]);
    let navigate = useNavigate(); 
    const routeChange = () =>{ 
        let path = `newPath`; 
        navigate(path);
    }

    function link(a: string) :string{
        var str = window.location.href = "version/" + a;
        // let path = a; 
        // navigate(path);
        return str;
    }

    useEffect(() => {
        const fetchGetAll = async () => {
            fetch('https://localhost:7001/api/Car/GetAll', {
                method: 'POST',
                headers: {
                    'Accept': 'application/json, text/plain, */*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({pagging: {page: page, count: count}})
                }).then(res => res.json())
                .then(res => {
                    setCar(res.cars);
                });
        }

        fetchGetAll();
    }, [page]);

    function IsAv(data: ICarData){
        if(data.data == true){
            return(
                <EventAvailableIcon />
            )
        }
        else {
            return (
                <EventBusyIcon />
            )
        }
    }

    return(
        
        <div>
            <Row xs={1} md={3} className="g-4">
                {cars.map((car) => (
                    <Col onClick={() => window.open(link(car.carId))}>
                    <Card sx={{ minWidth: "20%" }} key={car.carId}>
                    <img src={car.image} alt="React Image" className="imagecar" width={"100%"}/>
                    
            <CardContent>
                <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
                Book
                </Typography>
                <Typography variant="h5" component="div">
                {car.brand}
                </Typography>
                <Typography sx={{ mb: 1.5 }} color="text.secondary">
                {car.model}
                </Typography>
                <Typography variant="body2">
                License plate: {car.licensePlate}
                <br />
                Year Of Issue: {car.yearOfIssue}
                <br />
                Is Available: <IsAv data={car.isAvailable} />
                <br />
                Work Level: {car.level}
                </Typography>
            </CardContent>
            <CardActions>
                <Button onClick={() => window.open(link(car.carId))} size="small">See More</Button>
                <Button onClick={() => window.open(link(car.carId))} size="small">Book</Button>
            </CardActions>
        </Card>
                    </Col>
                ))}
    </Row>
        </div>
        
    );
}