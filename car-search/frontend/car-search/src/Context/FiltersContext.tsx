import React, { createContext, useState, useContext, ReactNode } from 'react';

// this is a context file, created so that the search results persist between pages

interface Filters {
    selectedBrand: string;
    selectedModel: string;
    selectedLocation: string;
    startDate: string;
    endDate: string;
}

interface FiltersContextValue {
    filters: Filters;
    setFilters: React.Dispatch<React.SetStateAction<Filters>>;
}

const FiltersContext = createContext<FiltersContextValue | undefined>(undefined);

const formatDate = (date: Date) => {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
};

export const FiltersProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const today = new Date();
    const tomorrow = new Date();
    tomorrow.setDate(today.getDate() + 1);

    const [filters, setFilters] = useState<Filters>({
        selectedBrand: '',
        selectedModel: '',
        selectedLocation: '',
        startDate: formatDate(today),
        endDate: formatDate(tomorrow),
    });

    return (
        <FiltersContext.Provider value={{ filters, setFilters }}>
            {children}
        </FiltersContext.Provider>
    );
};

export const useFilters = () => {
    const context = useContext(FiltersContext);
    if (!context) {
        throw new Error('useFilters must be used within a FiltersProvider');
    }
    return context;
};
