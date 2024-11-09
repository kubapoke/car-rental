import {useState, useEffect} from "react";
import CarOffer from './CarOffer.tsx'

const CarOffers = ({isHome = false}) => {
    const [offers, setOffers] = useState([]);

    useEffect(() => {
        const fetchOffers = async () => {
            const apiUrl = isHome ? '/api/cars?_limit=3' : '/api/cars';

            try {
                const res = await fetch(apiUrl);
                const data = await res.json();
                setOffers(data);
            }
            catch (error){
                console.log('Error fetching data', error);
            }
        }

        fetchOffers();
    }, []);
    
    return (
        <section className="bg-blue-50 px-4 py-10">
            <div className="container-xl lg:container m-auto">
                <h2 className="text-3xl font-bold text-indigo-500 mb-6 text-center">
                    You might like:
                </h2>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                        {offers.map((offer) => (
                            <CarOffer offer={offer}/>
                        ))}
                    </div>
            </div>
        </section>
    );
};

export default CarOffers;