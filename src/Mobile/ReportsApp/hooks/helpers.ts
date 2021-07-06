import {useState, useEffect} from 'react';
import {Dimensions} from 'react-native';

export type Orientation = 'portrait' | 'landscape' | null;
export type dim = 'window' | 'screen';

function useOrientation() {
  const [orientation, setOrientation] = useState<Orientation>(null);

  useEffect(() => {
    function getOrientation() {
      const dimensions = {
        width: Dimensions.get('window').width,
        height: Dimensions.get('window').height,
      };

      if (dimensions.width > dimensions.height) setOrientation('landscape');
      else setOrientation('portrait');
    }

    getOrientation(); //valor inicial
    Dimensions.addEventListener('change', getOrientation);
    return () => Dimensions.removeEventListener('change', getOrientation);
  }, []);

  return {orientation};
}

const useDimensions = (type: dim) => {
  const [dimensions, setDimensions] = useState(Dimensions.get(type));

  useEffect(() => {
    const currentDimensions = Dimensions.get(type);
    setDimensions(currentDimensions);
  }, [type]);

  useEffect(() => {
    function dimensionsChange(params: any) {
      setDimensions(params[type]);
    }

    Dimensions.addEventListener('change', dimensionsChange);
    return () => {
      Dimensions.removeEventListener('change', dimensionsChange);
    };
  }, [type]);

  return dimensions;
};

export {useOrientation, useDimensions};
