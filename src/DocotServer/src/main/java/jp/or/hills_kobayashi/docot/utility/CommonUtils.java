package jp.or.hills_kobayashi.docot.utility;

import org.apache.commons.codec.binary.Base32;
import org.springframework.beans.BeanWrapper;
import org.springframework.beans.BeanWrapperImpl;

import javax.xml.bind.DatatypeConverter;
import java.util.HashSet;
import java.util.Set;
import java.util.UUID;

public class CommonUtils {

    public static String[] getNullPropertyNames (Object source) {
        final BeanWrapper src = new BeanWrapperImpl(source);
        java.beans.PropertyDescriptor[] pds = src.getPropertyDescriptors();

        Set<String> emptyNames = new HashSet<String>();
        for(java.beans.PropertyDescriptor pd : pds) {
            Object srcValue = src.getPropertyValue(pd.getName());
            if (srcValue == null) emptyNames.add(pd.getName());
        }
        String[] result = new String[emptyNames.size()];
        return emptyNames.toArray(result);
    }

    public static String createBase32UUID() {
        UUID uuid = UUID.randomUUID();
        String uuidHex = uuid.toString().replaceAll("-", "");
        byte[] uuidBytes = DatatypeConverter.parseHexBinary(uuidHex);
        String uuidBase32 = new Base32().encodeAsString(uuidBytes);
        String uuidBase32WithoutPadding = uuidBase32.replaceAll("=", "");
        return uuidBase32WithoutPadding;
    }

}
