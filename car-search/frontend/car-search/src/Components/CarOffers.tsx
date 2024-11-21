import {useEffect} from "react";
import CarOffer from './CarOffer.tsx'
import {useOffers} from "../Context/OffersContext.tsx";
import {useFilters} from "../Context/FiltersContext.tsx";

const CarOffers = ({isHome} : {isHome : boolean}) => {
    const {offers, fetchOffers} = useOffers();
    const filters = useFilters().filters;

    useEffect(() => {
        fetchOffers({
            brand: filters.selectedBrand || '',
            model: filters.selectedModel || '',
            location: filters.selectedLocation || '',
            startDate: filters.startDate || '',
            endDate: filters.endDate || '',
        })
    }, [filters, fetchOffers]);

    // Limit offers to 3 if isHome is true
    const displayedOffers = isHome ? offers.slice(0, 3) : offers;

    const getHeader = () =>
    {
        if(isHome){
            if(offers.length > 0)
                return "Recently viewed:";
            else
                return "Tell us what you are looking for";
        }
        else{
            if(offers.length > 0)
                return "Search results:";
            else
                return "No offers matching your requirements have been found :(";
        }
    }
    
    return (
        <section className="bg-blue-50 px-4 py-10">
            <div className="container-xl lg:container m-auto">
                <h2 className="text-3xl font-bold text-indigo-500 mb-6 text-center">
                    {getHeader()}
                </h2>
                    <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                        {displayedOffers.map((offer) => (
                            <CarOffer key={offer.carId} offer={offer}/>
                        ))}
                    </div>
            </div>
        </section>
    );
};

export default CarOffers;